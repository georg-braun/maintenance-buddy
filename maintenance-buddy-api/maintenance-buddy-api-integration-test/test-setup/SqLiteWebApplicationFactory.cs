using System.Linq;
using budget_backend_integration_tests.backend;
using maintenance_buddy_api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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

        SetTokenValidationParameters(builder);
    }
    
    /// <summary>
    /// Modifies the existing Token Validation Parameters in the web server app.
    /// The result is that the web server accepts tokens issued from the test project.
    /// </summary>
    /// <param name="builder"></param>
    private void SetTokenValidationParameters(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                // modify the token validation parameters in the server configuration
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = TestTokenIssuer.SecurityKey,
                    ValidIssuer = TestTokenIssuer.Issuer,
                    ValidAudience = TestTokenIssuer.Audience
                };
            });
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