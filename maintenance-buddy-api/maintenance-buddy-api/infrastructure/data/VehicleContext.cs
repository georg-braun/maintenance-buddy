using maintenance_buddy_api.domain;
using Microsoft.EntityFrameworkCore;

namespace maintenance_buddy_api;

public class VehicleContext : DbContext
{
    public VehicleContext(DbContextOptions options) : base(options)
    {
    }

    private DbSet<Vehicle> Vehicles { get; }

    public IEnumerable<Vehicle> GetVehicles()
    {
        return Vehicles.ToList();
    }

    public async Task AddVehicle(Vehicle vehicle)
    {
        await Vehicles.AddAsync(vehicle);
        await SaveChangesAsync();
    }
}