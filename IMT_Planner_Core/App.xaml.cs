using System.Windows;
using IMT_Planner_DAL.Context;
using Microsoft.Extensions.DependencyInjection;
using IMT_Planner_DAL.Repositories;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.ChronoViewModels;
using IMT_Planner_ViewModels.GeneralViewModels;
using IMT_Planner_ViewModels.MainViewModels;
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
        services.AddScoped<IRepository<SuperManager>, SuperManagerRepository>(); 
        services.AddDbContext<IMTPlannerDbContext>(options =>
        {
            options.UseSqlite("Data Source=IMT_Planner.db");
            options.EnableSensitiveDataLogging();
            
        });
        // Register your services and ViewModels here
        services.AddSingleton<IMT_Planner_ViewModels.Services.SuperManagerSelectionService>();
        services.AddSingleton<IMT_Planner_ViewModels.Services.SuperManagerRepositoryService>();
        services.AddSingleton<IMT_Planner_DAL.CSVHandler>();
        
        
        services.AddTransient<SuperManagerPlannerViewModel>();
        services.AddTransient<SuperManagerDetailsViewModel>();
        services.AddSingleton<SuperManagerListViewModel>();
        services.AddSingleton<SuperManagerElementViewModel>();
        services.AddSingleton<SMElementalListViewModel>();
        services.AddSingleton<FilterViewModel>();
        services.AddSingleton<ChronoMineEntityViewModel>();
        // Add other services or view models as needed.
        
        

    }

}