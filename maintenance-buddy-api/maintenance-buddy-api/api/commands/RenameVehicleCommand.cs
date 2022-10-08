namespace maintenance_buddy_api.api.commands;

public record RenameVehicleCommand
(
    string VehicleId,
    string Name
);