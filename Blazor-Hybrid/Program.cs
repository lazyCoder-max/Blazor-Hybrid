using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MudBlazor.Services;
using Shared;
using MudBlazor;

namespace Blazor_Hybrid
{
    internal static class Program
    {
        public static ServiceCollection Services { get; set; } = null!;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("Config\\appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();
            
            Services = new ServiceCollection();
            Services.AddWindowsFormsBlazorWebView();
            Services.AddMudServices();
            Services.AddMudMarkdownServices();
            Services.AddPromanAISharedServices(configuration);

            Application.Run(new Form1());
        }
    }
}