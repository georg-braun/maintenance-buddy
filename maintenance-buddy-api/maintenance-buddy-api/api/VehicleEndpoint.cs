using maintenance_buddy_api.api.commands;
using maintenance_buddy_api.domain;

namespace maintenance_buddy_api.api;

public static class VehicleEndpoint
{
    public static async Task<IResult> CreateVehicle(CreateVehicleCommand command, VehicleContext context)
    {
        var vehicle = VehicleFactory.Create(command.Name, command.Kilometer);
        context.AddVehicle(vehicle);
        return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
    }
}