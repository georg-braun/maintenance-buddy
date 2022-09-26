using System.Security.Claims;
using maintenance_buddy_api.api.commands;
using maintenance_buddy_api.api.dto;
using maintenance_buddy_api.domain;
using Microsoft.EntityFrameworkCore;

namespace maintenance_buddy_api.api;

public static class VehicleEndpoint
{
    public static async Task<IResult> CreateVehicle(CreateVehicleCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicle = VehicleFactory.Create(command.Name, command.Kilometer);

        context.AddVehicle(vehicle);
        await context.ConnectVehicleAndUser(userId, vehicle.Id);
        await context.SaveChangesAsync();

        return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
    }    
    
    public static async Task<IResult> AddActionTemplate(AddActionTemplateCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleId = new Guid(command.VehicleId);
       
        var vehicle = (await context.GetVehicles(userId)).FirstOrDefault(_ => _.Id.Equals(vehicleId));

        if (vehicle is null)
            return Results.NotFound("Vehicle not found");
        
        var actionTemplate = vehicle.AddActionTemplate(command.Name, command.KilometerInterval, command.TimeInterval);
        context.UpdateVehicle(vehicle);
        
        await context.SaveChangesAsync();

        return Results.Created($"/vehicle/{vehicle.Id}", actionTemplate);
    }
    
    public static async Task<IResult> DeleteActionTemplate(DeleteActionTemplateCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleId = new Guid(command.VehicleId);
        var actionTemplateId = new Guid(command.ActionTemplateId);
        
        var vehicle = (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates).FirstOrDefault(_ => _.Id.Equals(vehicleId));
        if (vehicle is null)
            return Results.NotFound();
        
        vehicle.RemoveActionTemplate(actionTemplateId);
        
        context.Update(vehicle);
        await context.SaveChangesAsync();
        
        return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
    }
    
    public static async Task<IResult> AddAction(AddActionCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        if (string.IsNullOrEmpty(command.VehicleId) || string.IsNullOrEmpty(command.ActionTemplateId))
            return Results.Problem("VehicleId or TemplateId isn't filled");
        var userId = ExtractUserId(claims);
        var vehicleId = new Guid(command.VehicleId);
        var actionTemplateId = new Guid(command.ActionTemplateId);
        var utcDate = command.Date.ToUniversalTime();

        var vehicle = await (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleId));

        if (vehicle is null)
            return Results.NotFound();

        var action = vehicle.AddAction(actionTemplateId, utcDate, command.Kilometer, command.Note);
        if (action is null)
            return Results.UnprocessableEntity("Action couldn't be created");
        
        context.UpdateVehicle(vehicle);
        
        await context.SaveChangesAsync();

        return Results.Created($"/action/{action.Id}", action);
    }
    
    public static async Task<IResult> DeleteAction(DeleteActionCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleId = new Guid(command.VehicleId);
        var actionTemplateId = new Guid(command.ActionTemplateId);
        var actionId = new Guid(command.ActionId);

        var vehicle = await (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates).ThenInclude(_ => _.Actions)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleId));

        if (vehicle is null)
            return Results.NotFound();

        vehicle.DeleteAction(actionTemplateId, actionId);
        context.UpdateVehicle(vehicle);
        
        await context.SaveChangesAsync();

        return Results.Ok();
    }

    public static async Task<IResult> ActionTemplatesQuery(string vehicleId, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleGuid = new Guid(vehicleId);
        var vehicle = await (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates).FirstOrDefaultAsync(_ => _.Id.Equals(vehicleGuid));

        return vehicle is null ? Results.NotFound() : Results.Ok(vehicle.ActionTemplates);
    }

    public static async Task<IResult> ActionsQuery(string vehicleId, string ActionTemplateId, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleGuid = new Guid(vehicleId);
        var actionTemplateId = new Guid(ActionTemplateId);

        var vehicle = await (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates).ThenInclude(_ => _.Actions)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleGuid));

        if (vehicle is null)
            return Results.NotFound("Vehicle not found.");

        var actions = vehicle.GetActions(actionTemplateId);
        var actionDtos = actions.Select(ActionDtoMapper.ToDto);

        return Results.Ok(actionDtos);
    }
    
    public static async Task<IResult> ActionsOfVehicleQuery(string vehicleId, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleGuid = new Guid(vehicleId);

        var vehicle = await (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates).ThenInclude(_ => _.Actions)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleGuid));

        if (vehicle is null)
            return Results.NotFound("Vehicle not found.");

        var actions = vehicle.GetActions();
        var actionDtos = actions.Select(ActionDtoMapper.ToDto);

        return Results.Ok(actionDtos);
    }
    
    public static async Task<IResult> VehiclesQuery(VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicles = await context.GetVehicles(userId);
        
        return Results.Ok(vehicles);
    }

    private static string ExtractUserId(ClaimsPrincipal claims)
    {
        return claims.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}