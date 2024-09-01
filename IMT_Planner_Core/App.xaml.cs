using System.Configuration;
using System.Data;
using System.Windows;
using IMT_Planner_DAL;
using IMT_Planner_DAL.Context;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using IMT_Planner_DAL.Repositories;
using Microsoft.EntityFrameworkCore;

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

        using(var scope = ServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IMTPlannerDbContext>();
            dbContext.Database.Migrate();
        }
        
    }

    private void ConfigureServices(IServiceCollection services)
    {
        //Repos
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddDbContext<IMTPlannerDbContext>(options =>
        {
            options.UseSqlite("Data Source=IMT_Planner.db");
            options.EnableSensitiveDataLogging();
            
        });
        // Register your services and ViewModels here
        services.AddSingleton<IMT_Planner_ViewModels.Services.SuperManagerSelectionService>();
        services.AddSingleton<IMT_Planner_ViewModels.Services.SuperManagerRepositoryService>();
        services.AddSingleton<IMT_Planner_DAL.CSVHandler>();
        
        
        services.AddTransient<IMT_Planner_ViewModels.SuperManagerDetailsViewModel>();
        services.AddSingleton<IMT_Planner_ViewModels.SuperManagerListViewModel>();
        services.AddSingleton<IMT_Planner_ViewModels.SuperManagerElementViewModel>();
        services.AddSingleton<IMT_Planner_ViewModels.SMElementalListViewModel>();
        // Add other services or view models as needed.
        
        

    }

}