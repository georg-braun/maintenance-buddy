using System.Security.Claims;
using maintenance_buddy_api.api.commands;
using maintenance_buddy_api.domain;
using Microsoft.EntityFrameworkCore;

namespace maintenance_buddy_api.api;

public static class VehicleEndpoint
{
    public static async Task<IResult> CreateVehicle(CreateVehicleCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        Console.WriteLine(claims.FindFirstValue(ClaimTypes.NameIdentifier));
        var vehicle = VehicleFactory.Create(command.Name, command.Kilometer);
        
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync();

        return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
    }    
    
    public static async Task<IResult> AddActionTemplate(AddActionTemplateCommand command, VehicleContext context)
    {
        var vehicleId = new Guid(command.VehicleId);
       
        var vehicle = await context.Vehicles.FindAsync(vehicleId);

        if (vehicle is null)
            return Results.NotFound("Vehicle not found");
        
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
        if (vehicle is null)
            return Results.NotFound();
        
        vehicle.RemoveActionTemplate(actionTemplateId);
        
        context.Vehicles.Update(vehicle);
        await context.SaveChangesAsync();
        
        return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
    }
    
    public static async Task<IResult> AddAction(AddActionCommand command, VehicleContext context)
    {
        var vehicleId = new Guid(command.VehicleId);
        var actionTemplateId = new Guid(command.ActionTemplateId);

        var vehicle = await context.Vehicles.Include(_ => _.ActionTemplates)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleId));

        if (vehicle is null)
            return Results.NotFound();

        var action = vehicle.AddAction(actionTemplateId, command.Date, command.Kilometer, command.Note);
        if (action is null)
            return Results.UnprocessableEntity("Action couldn't be created");
        
        context.Vehicles.Update(vehicle);
        
        await context.SaveChangesAsync();

        return Results.Created($"/action/{action.Id}", action);
    }
    
    public static async Task<IResult> DeleteAction(DeleteActionCommand command, VehicleContext context)
    {
        var vehicleId = new Guid(command.VehicleId);
        var actionTemplateId = new Guid(command.ActionTemplateId);
        var actionId = new Guid(command.ActionId);

        var vehicle = await context.Vehicles.Include(_ => _.ActionTemplates).ThenInclude(_ => _.Actions)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleId));

        if (vehicle is null)
            return Results.NotFound();

        vehicle.DeleteAction(actionTemplateId, actionId);
        context.Vehicles.Update(vehicle);
        
        await context.SaveChangesAsync();

        return Results.Ok();
    }

    public static async Task<IResult> ActionTemplatesQuery(string vehicleId, VehicleContext context)
    {
        var vehicleGuid = new Guid(vehicleId);
        var vehicle = await context.Vehicles.Include(_ => _.ActionTemplates).FirstOrDefaultAsync(_ => _.Id.Equals(vehicleGuid));

        return vehicle is null ? Results.NotFound() : Results.Ok(vehicle.ActionTemplates);
    }

    public static async Task<IResult> ActionsQuery(string vehicleId, string ActionTemplateId, VehicleContext context)
    {
        var vehicleGuid = new Guid(vehicleId);
        var actionTemplateId = new Guid(ActionTemplateId);

        var vehicle = await context.Vehicles.Include(_ => _.ActionTemplates).ThenInclude(_ => _.Actions)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleGuid));

        if (vehicle is null)
            return Results.NotFound("Vehicle not found.");

        var actions = vehicle.GetActions(actionTemplateId);

        return Results.Ok(actions);
    }
}