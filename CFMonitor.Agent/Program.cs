
using System.ComponentModel;
using System.Net;
using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CFWebServerConsole
{
    /// <summary>
    /// NOTES:    
    /// - Each Site instance serves one website. We pass in seperate dependencies for each because each site 
    ///   is independent.
    /// - We create an internal website which handles site config requests. E.g. Add site, update site permissions.
    /// - Logs are separate per website.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            var serviceProvider = CreateServiceProvider();

            // Wait for requrest to stop
            // TODO: Consider supporting a command line argument /STOP so that user can run a second instance of this process
            // that tells the first instance to stop and then terminates.
            do
            {
                Console.WriteLine("Press ESCAPE to stop");  // Also displayed if user presses other key
                while (!Console.KeyAvailable)
                {
                    Thread.Sleep(100);
                    Thread.Yield();
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            Console.WriteLine("Terminating CF Web Server");


            Console.WriteLine("Terminated CF Web Server");
        }


        private static IServiceProvider CreateServiceProvider()
        {
            var configFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Config");
            var logFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Log");

            var configuration = new ConfigurationBuilder()
                .Build();

            var serviceProvider = new ServiceCollection()              
                .BuildServiceProvider();

            return serviceProvider;
        }

        ///// <summary>
        ///// Registers all types implementing interface
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="services"></param>
        ///// <param name="assemblies"></param>
        ///// <param name="lifetime"></param>
        //private static void RegisterAllTypes<T>(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        //{
        //    var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
        //    foreach (var type in typesFromAssemblies)
        //    {
        //        services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
        //    }
        //}     
    }
}

