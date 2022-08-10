using System.Linq;
using maintenance_buddy_api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace maintenance_buddy_api_integration_test;

/// <summary>
///     The modified web application factory that uses a sqlite database.
/// </summary>
/// <typeparam name="TStartup"></typeparam>
public class SqLiteWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    private readonly SqliteConnection _connection;

    public SqLiteWebApplicationFactory(SqliteConnection connection)
    {
        _connection = connection;
    }

    private void RemoveRegisteredDataContext(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType ==
                 typeof(DbContextOptions<VehicleContext>));

        if (descriptor is null) return;
        services.Remove(descriptor);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            RemoveRegisteredDataContext(services);
            AddSqliteDbContext(services);
            EnsureThatDatabaseIsCreated(services);
        });
    }
    

    private void EnsureThatDatabaseIsCreated(IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();

        using var scope = sp.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<VehicleContext>();

        db.Database.EnsureCreated();
    }

    private void AddSqliteDbContext(IServiceCollection services)
    {
        services.AddDbContext<VehicleContext>(options => { options.UseSqlite(_connection); });
    }
}