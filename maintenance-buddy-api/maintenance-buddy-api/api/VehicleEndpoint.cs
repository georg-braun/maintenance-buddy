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
        var actionTemplate = vehicle.AddActionTemplate(command.Name, command.KilometerInterval, command.TimeInterval);
        context.Vehicles.Update(vehicle);
        
        await context.SaveChangesAsync();

        return Results.Created($"/vehicle/{vehicle.Id}", actionTemplate);
    }
    
    public static async Task<IResult> DeleteActionTemplate(DeleteActionTemplateCommand command, VehicleContext context)
    {
        var vehicleId = new Guid(command.VehicleId);
        var actionTemplateId = new Guid(command.ActionTemplateId);
        
        var vehicle = context.Vehicles.Include(_ => _.ActionTemplates).FirstOrDefault(_ => _.Id.Equals(vehicleId));
        vehicle.RemoveActionTemplate(actionTemplateId);
        
        context.Vehicles.Update(vehicle);
        await context.SaveChangesAsync();
        
        return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
    }

    public static async Task<IResult> ActionTemplatesQuery(string vehicleId, VehicleContext context)
    {
        var vehicleGuid = new Guid(vehicleId);
        var vehicle = context.Vehicles.Include(_ => _.ActionTemplates).FirstOrDefault(_ => _.Id.Equals(vehicleGuid));

        return vehicle is null ? Results.NotFound() : Results.Ok(vehicle.ActionTemplates);
    }
}