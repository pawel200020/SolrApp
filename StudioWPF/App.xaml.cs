using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace StudioWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //[STAThread]
        //public static void Main(string[] args)
        //{
        //    using IHost host = CreateHostBuilder(args).Build();
        //    App app = new();
        //    app.InitializeComponent();
        //    app.MainWindow = host.Services.GetRequiredService<MainWindow>();
        //    app.MainWindow.Visibility = Visibility.Visible;
        //    app.Run();
        //}

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
        //    {

        //    }).ConfigureServices((hostContext, services) =>
        //    {
        //        services.AddSingleton<MainWindow>();

        //    });
    }
}
