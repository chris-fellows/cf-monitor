using CFMonitor.Interfaces;
using CFMonitor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Windows.Forms;

namespace CFMonitor
{
    static class Program
    {       
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(ServiceProvider.GetRequiredService<MainForm>());
        }

        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Create a host builder to build the service provider
        /// </summary>
        /// <returns></returns>
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.AddTransient<IMonitorItemControlService, MonitorItemControlService>();
                    services.AddTransient<IMonitorItemTypeService, MonitorItemTypeService>();                    
                    services.AddTransient<IMonitorAgentService>((scope) =>
                    {
                        return new MonitorAgentService(Path.Combine(Environment.CurrentDirectory, "Data", "MonitorAgents"));
                    });
                    services.AddTransient<IMonitorItemService>((scope) =>
                    {
                        return new MonitorItemService(Path.Combine(Environment.CurrentDirectory, "Data", "MonitorItems"));
                    });                    
                    services.AddTransient<MainForm>();
                });
        }
    }
}
