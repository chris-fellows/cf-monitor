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

namespace CFMonitor.Agent
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting CF Monitor Agent");

            var serviceProvider = CreateServiceProvider();

            // Start worker
            int localPort = 1000;
            var worker = new Worker(localPort);
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
                .AddScoped<IMonitorAgentService, XmlMonitorAgentService>()
                .AddScoped<IMonitorItemService, XmlMonitorItemService>()
                .AddScoped<IUserService, XmlUserService>()
                .AddScoped<IMonitorItemTypeService, MonitorItemTypeService>()
                .AddScoped<ISystemValueTypeService, XmlSystemValueTypeService>()

                //.AddScoped<IActionersService, ActionersService>()
                //.AddScoped<ICheckersService, CheckersService>()

                // Seed                
                .AddKeyedScoped<IEntityReader<EventItem>, EventItemSeed1>("EventItemSeed1")
                .AddKeyedScoped<IEntityReader<MonitorItem>, MonitorItemSeed1>("MonitorItemSeed1")
                .AddKeyedScoped<IEntityReader<SystemValueType>, SystemValueTypeSeed1>("SystemValueTypeSeed1")
                .AddKeyedScoped<IEntityReader<User>, UserSeed1>("UserSeed1")                

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

