using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Shared;

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
            Services = new ServiceCollection();
            Services.AddWindowsFormsBlazorWebView();
            Services.AddMudServices();
            Services.AddPromanAISharedServices();
            Application.Run(new Form1());
        }
    }
}