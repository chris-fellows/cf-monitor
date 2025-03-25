using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Seed;
using CFMonitor.Services;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CFMonitor.Agent.Models;
using CFMonitor.SystemTask;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CFMonitor.Agent
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting CF Monitor Agent");

            var serviceProvider = CreateServiceProvider();

            // Set agent config
            var systemConfig = new SystemConfig()
            {
                LocalPort = 10001,
                MaxConcurrentChecks = 3,
                MonitorItemFilesRootFolder = "D:\\Data\\Dev\\C#\\cf-monitor-local\\MonitorAgent-1"                
            };

            // Start worker
            var worker = new Worker(serviceProvider, systemConfig);
            worker.Start();

            // Wait for requrest to stop            
            do
            {
                Console.WriteLine("Press ESCAPE to stop");  // Also displayed if user presses other key
                while (!Console.KeyAvailable)
                {
                    Thread.Sleep(100);
                    Thread.Yield();
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            // Stop worker
            worker.Stop();

            Console.WriteLine("Terminated CF Monitor Agent");
        }


        private static IServiceProvider CreateServiceProvider()
        {
            var configFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Config");
            var logFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Log");

            var configuration = new ConfigurationBuilder()                
                .Build();

            var serviceProvider = new ServiceCollection()
                // Add data services
                .AddScoped<IActionItemTypeService>((scope) =>
                {
                    return new XmlActionItemTypeService(Path.Combine(configFolder, "ActionItemType"));
                })
                .AddScoped<IAuditEventService>((scope) =>
                {
                    return new XmlAuditEventService(Path.Combine(configFolder, "AuditEvent"));
                })
                .AddScoped<IAuditEventTypeService>((scope) =>
                {
                    return new XmlAuditEventTypeService(Path.Combine(configFolder, "AuditEventType"));
                })
                .AddScoped<IEventItemService>((scope) =>
                {
                    return new XmlEventItemService(Path.Combine(configFolder, "EventItem"));
                })
                .AddScoped<IFileObjectService>((scope) =>
                {
                    return new XmlFileObjectService(Path.Combine(configFolder, "FileObject"));
                })
                .AddScoped<IMonitorAgentService>((scope) =>
                {
                    return new XmlMonitorAgentService(Path.Combine(configFolder, "MonitorAgent"));
                })
                .AddScoped<IMonitorAgentGroupService>((scope) =>
                {
                    return new XmlMonitorAgentGroupService(Path.Combine(configFolder, "MonitorAgentGroup"));
                })
                   .AddScoped<IMonitorItemOutputService>((scope) =>
                   {
                       return new XmlMonitorItemOutputService(Path.Combine(configFolder, "MonitorItemOutput"));
                   })
                .AddScoped<IMonitorItemService>((scope) =>
                {
                    return new XmlMonitorItemService(Path.Combine(configFolder, "MonitorItem"));
                })
                   .AddScoped<IPasswordResetService>((scope) =>
                   {
                       return new XmlPasswordResetService(Path.Combine(configFolder, "PasswordReset"));
                   })
                .AddScoped<ISystemValueTypeService>((scope) =>
                {
                    return new XmlSystemValueTypeService(Path.Combine(configFolder, "SystemValueType"));
                })
                .AddScoped<IUserService>((scope) =>
                {
                    return new XmlUserService(Path.Combine(configFolder, "User"), scope.GetRequiredService<IPasswordService>());
                })

                .AddScoped<IPasswordService, PBKDF2PasswordService>()   // Needed for IUserService
                .AddScoped<IAuditEventFactory, AuditEventFactory>()
                .AddScoped<IMonitorItemTypeService, MonitorItemTypeService>()

                .RegisterAllTypes<IChecker>(new[] { typeof(Program).Assembly, typeof(MonitorItem).Assembly }, ServiceLifetime.Scoped)
                .RegisterAllTypes<IActioner>(new[] { typeof(Program).Assembly, typeof(MonitorItem).Assembly }, ServiceLifetime.Scoped)
                .RegisterAllTypes<ISystemTask>(new[] { typeof(Program).Assembly, typeof(MonitorItem).Assembly }, ServiceLifetime.Scoped)

                // Add system tasks
                .AddSingleton<ISystemTaskList>((scope) =>
                {
                    var systemTaskConfigs = new List<SystemTaskConfig>()
                    {
                        new SystemTaskConfig()
                        {
                            
                        }
                    };
                    return new SystemTaskList(4, systemTaskConfigs);
                })

                //// Seed                
                //.AddKeyedScoped<IEntityReader<ActionItemType>, ActionItemTypeSeed1>("ActionItemTypeSeed1")
                //.AddKeyedScoped<IEntityReader<AuditEventType>, AuditEventTypeSeed1>("AuditEventTypeSeed1")
                //.AddKeyedScoped<IEntityReader<EventItem>, EventItemSeed1>("EventItemSeed1")
                //.AddKeyedScoped<IEntityReader<MonitorAgent>, MonitorAgentSeed1>("MonitorAgentSeed1")
                //.AddKeyedScoped<IEntityReader<MonitorItem>, MonitorItemSeed1>("MonitorItemSeed1")
                //.AddKeyedScoped<IEntityReader<SystemValueType>, SystemValueTypeSeed1>("SystemValueTypeSeed1")
                //.AddKeyedScoped<IEntityReader<User>, UserSeed1>("UserSeed1")                

                .BuildServiceProvider();            

            return serviceProvider;
        }

        /// <summary>
        /// Registers all types implementing interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <param name="lifetime"></param>
        private static IServiceCollection RegisterAllTypes<T>(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
            foreach (var type in typesFromAssemblies)
            {
                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
            }

            return services;
        }
    }
}

