using maintenance_buddy_api.api.commands;
using maintenance_buddy_api.domain;
using Microsoft.EntityFrameworkCore;

namespace maintenance_buddy_api.api;

public static class VehicleEndpoint
{
    public static async Task<IResult> CreateVehicle(CreateVehicleCommand command, VehicleContext context)
    {
        var vehicle = VehicleFactory.Create(command.Name, command.Kilometer);
        
        context.Vehicles.Add(vehicle);
        context.SaveChangesAsync();

        return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
    }    
    
    public static async Task<IResult> AddActionTemplate(AddActionTemplateCommand command, VehicleContext context)
    {
        var vehicleId = new Guid(command.VehicleId);
       
        var vehicle = await context.Vehicles.FindAsync(vehicleId);
        vehicle.AddActionTemplate(command.Name, command.KilometerInterval, command.TimeInterval);
        context.Vehicles.Update(vehicle);
        
        await context.SaveChangesAsync();

        return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
    }
}