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

        return Results.Created($"/vehicle/{vehicle.Id}", ActionTemplateDtoMapper.ToDto(actionTemplate));
    }
    
    public static async Task<IResult> RenameVehicle(RenameVehicleCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleId = new Guid(command.VehicleId);
       
        var vehicle = (await context.GetVehicles(userId)).FirstOrDefault(_ => _.Id.Equals(vehicleId));

        if (vehicle is null)
            return Results.NotFound("Vehicle not found");

        vehicle.Rename(command.Name);
        context.Update(vehicle);
        await context.SaveChangesAsync();

        return Results.Ok("Vehicle name changed");
    }
    
    public static async Task<IResult> ChangeVehicleKilometer(ChangeVehicleKilometerCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleId = new Guid(command.VehicleId);
       
        var vehicle = (await context.GetVehicles(userId)).FirstOrDefault(_ => _.Id.Equals(vehicleId));

        if (vehicle is null)
            return Results.NotFound("Vehicle not found");

        vehicle.ChangeKilometer(command.Kilometer);
        context.Update(vehicle);
        await context.SaveChangesAsync();

        return Results.Ok("Vehicle name changed");
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

    public async static Task<IResult> DeleteVehicle(VehicleContext context, ClaimsPrincipal claims, string vehicleId)
    {
        var userId = ExtractUserId(claims);
        var vehicleGuid = new Guid(vehicleId);
        var vehicles = (await context.GetVehicles(userId)).ToList();

        var vehicle = vehicles.FirstOrDefault(_ => _.Id.Equals(vehicleGuid));

        if (vehicle is null)
            return Results.Problem("Couldn't find that vehicle.");

        context.Remove(vehicle);
        await context.SaveChangesAsync();
        
        return Results.Ok("vehicle deleted");
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

        return Results.Created($"/action/{action.Id}", ActionDtoMapper.ToDto(action));
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
    
    public static async Task<IResult> ChangeActionKilometer(ChangeActionKilometerCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleId = new Guid(command.VehicleId);
        var actionTemplateId = new Guid(command.ActionTemplateId);
        var actionId = new Guid(command.ActionId);

        var vehicle = await (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates).ThenInclude(_ => _.Actions)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleId));

        if (vehicle is null)
            return Results.NotFound();
        
        vehicle.ChangeActionKilometer(actionTemplateId, actionId, command.Kilometer);
        context.UpdateVehicle(vehicle);
        
        await context.SaveChangesAsync();

        return Results.Ok();
    }
    
    public static async Task<IResult> ChangeActionTemplateName(ChangeActionTemplateNameCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleId = new Guid(command.VehicleId);
        var actionTemplateId = new Guid(command.ActionTemplateId);
        

        var vehicle = await (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates).ThenInclude(_ => _.Actions)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleId));

        if (vehicle is null)
            return Results.NotFound();
        
        vehicle.ChangeActionTemplateName(actionTemplateId, command.Name);
        context.UpdateVehicle(vehicle);
        
        await context.SaveChangesAsync();

        return Results.Ok();
    }
    
    public static async Task<IResult> ChangeActionTemplateKilometerInterval(ChangeActionTemplateKilometerIntervalCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleId = new Guid(command.VehicleId);
        var actionTemplateId = new Guid(command.ActionTemplateId);
        

        var vehicle = await (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates).ThenInclude(_ => _.Actions)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleId));

        if (vehicle is null)
            return Results.NotFound();
        
        vehicle.ChangeActionTemplateKilometerInterval(actionTemplateId, command.KilometerInterval);
        context.UpdateVehicle(vehicle);
        
        await context.SaveChangesAsync();

        return Results.Ok();
    }
    
    public static async Task<IResult> ChangeActionTemplateTimeInterval(ChangeActionTemplateTimeIntervalCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleId = new Guid(command.VehicleId);
        var actionTemplateId = new Guid(command.ActionTemplateId);
        

        var vehicle = await (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates).ThenInclude(_ => _.Actions)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleId));

        if (vehicle is null)
            return Results.NotFound();
        
        vehicle.ChangeActionTemplateTimeInterval(actionTemplateId, command.TimeInterval);
        context.UpdateVehicle(vehicle);
        
        await context.SaveChangesAsync();

        return Results.Ok();
    }
    
    public static async Task<IResult> ChangeActionNote(ChangeActionNoteCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleId = new Guid(command.VehicleId);
        var actionTemplateId = new Guid(command.ActionTemplateId);
        var actionId = new Guid(command.ActionId);

        var vehicle = await (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates).ThenInclude(_ => _.Actions)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleId));

        if (vehicle is null)
            return Results.NotFound();
        
        vehicle.ChangeActionNote(actionTemplateId, actionId, command.Note);
        context.UpdateVehicle(vehicle);
        
        await context.SaveChangesAsync();

        return Results.Ok();
    }
    
    public static async Task<IResult> ChangeActionDate(ChangeActionDateCommand command, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleId = new Guid(command.VehicleId);
        var actionTemplateId = new Guid(command.ActionTemplateId);
        var actionId = new Guid(command.ActionId);

        var vehicle = await (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates).ThenInclude(_ => _.Actions)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleId));

        if (vehicle is null)
            return Results.NotFound();
        
        vehicle.ChangeActionDate(actionTemplateId, actionId, command.Date);
        context.UpdateVehicle(vehicle);
        
        await context.SaveChangesAsync();

        return Results.Ok();
    }

    public static async Task<IResult> ActionTemplatesQuery(string vehicleId, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleGuid = new Guid(vehicleId);
        var vehicle = await (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates).FirstOrDefaultAsync(_ => _.Id.Equals(vehicleGuid));

        var dtos = vehicle?.ActionTemplates.Select(ActionTemplateDtoMapper.ToDto);
        return vehicle is null ? Results.NotFound() : Results.Ok(dtos);
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
    
    public static async Task<IResult> VehiclePendingActionsQuery(string vehicleId, VehicleContext context, ClaimsPrincipal claims)
    {
        var userId = ExtractUserId(claims);
        var vehicleGuid = new Guid(vehicleId);

        var vehicle = await (await context.GetVehicles(userId)).Include(_ => _.ActionTemplates).ThenInclude(_ => _.Actions)
            .FirstOrDefaultAsync(_ => _.Id.Equals(vehicleGuid));

        if (vehicle is null)
            return Results.NotFound("Vehicle not found.");

        var pendingActions = vehicle.GetPendingActions(DateTime.Today.ToUniversalTime());
        return Results.Ok(pendingActions);
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