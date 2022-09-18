using System.Linq.Expressions;
using maintenance_buddy_api.domain;
using maintenance_buddy_api.infrastructure.database_model;
using Microsoft.EntityFrameworkCore;

namespace maintenance_buddy_api;

public class VehicleContext : DbContext
{
    public VehicleContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // The ActionTemplate Id isn't auto generated. This specification is important because otherwise
        // modifications of the action templates in the vehicle aggregate aren't detected by ef
        // further information: https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.x/breaking-changes#detectchanges-honors-store-generated-key-values
        modelBuilder.Entity<ActionTemplate>().Property(at => at.Id).ValueGeneratedNever();
        modelBuilder.Entity<domain.Action>().Property(at => at.Id).ValueGeneratedNever();

        modelBuilder.Entity<UserVehicleConnection>().HasKey(_ => new {_.NameIdentifier, _.VehicleId});
    }

    private DbSet<Vehicle> Vehicles { get; set; } = null!;
    
    public DbSet<UserVehicleConnection> UserVehicleConnections { get; set; } = null!;

    public async Task<List<Guid>> GetAvailableVehiclesOfUserAsync(string nameIdentifier)
    {
        return await UserVehicleConnections.Where(_ => _.NameIdentifier.Equals(nameIdentifier)).Select(_ => _.VehicleId).ToListAsync();
    }
    
    public async Task ConnectVehicleAndUser(string userId, Guid vehicleId)
    {
        var connection = new UserVehicleConnection(userId, vehicleId);
        await UserVehicleConnections.AddAsync(connection);
    }

    public async Task<IQueryable<Vehicle>> GetVehicles(string userId)
    {
        var availableVehicles = await GetAvailableVehiclesOfUserAsync(userId);
        return Vehicles.Where(_ => availableVehicles.Contains(_.Id));
    }

    public void UpdateVehicle(Vehicle vehicle)
    {
        Vehicles.Update(vehicle);
    }

    public void AddVehicle(Vehicle vehicle)
    {
        Vehicles.Add(vehicle);
    }
}