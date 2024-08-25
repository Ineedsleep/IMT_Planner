using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;


namespace IMT_Planner;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    public IServiceProvider ServiceProvider { get; private set; }
    
    protected override void OnStartup(StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();

        base.OnStartup(e);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        
        // Register your services and ViewModels here
        services.AddSingleton<IMT_Planner_ViewModels.Services.SuperManagerService>();
        
        
        services.AddTransient<IMT_Planner_ViewModels.SuperManagerViewModel>();
        services.AddSingleton<IMT_Planner_ViewModels.SuperManagerListViewModel>();
        services.AddSingleton<IMT_Planner_ViewModels.SuperManagerElementViewModel>();
        // Add other services or view models as needed.
    }

}