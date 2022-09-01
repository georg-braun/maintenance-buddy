using maintenance_buddy_api.domain;
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
    }

    public DbSet<Vehicle> Vehicles { get; set; }
    
    public DbSet<ActionTemplate> ActionTemplates { get; set; }

    public IEnumerable<Vehicle> GetVehicles()
    {
        return Vehicles.ToList();
    }

    public async Task AddVehicle(Vehicle vehicle)
    {
        await Vehicles.AddAsync(vehicle);

    }

    public async Task<Vehicle> GetVehicleAsync(Guid vehicleId)
    {
        //var vehicle = await Vehicles.FirstOrDefaultAsync(_ => _.Id.Equals(vehicleId));
        var vehicle = await Vehicles.FindAsync(vehicleId);
        return vehicle;
    }

    public void UpdateVehicle(Vehicle vehicle)
    {
        Vehicles.Update(vehicle);
    }
}