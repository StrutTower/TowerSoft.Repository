using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TowerSoft.RepositoryExample.Jobs;
using TowerSoft.RepositoryExample.Repository;

// Set up dependency injection (DI) for a console app
ServiceProvider services = ConfigureServices();

// Get a job from DI
services.GetService<TestJob>().StartJob();



static ServiceProvider ConfigureServices() {
    IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
#if DEBUG
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
#else
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true)
#endif
                .Build();

    IServiceCollection services = new ServiceCollection();

    services
        .AddSingleton(configuration)
        .AddScoped<TestJob>()
        .AddScoped<UnitOfWork>(); // Add the UnitOfWork as a scoped service


    ServiceProvider serviceProvider = services.BuildServiceProvider();
    return serviceProvider;
}