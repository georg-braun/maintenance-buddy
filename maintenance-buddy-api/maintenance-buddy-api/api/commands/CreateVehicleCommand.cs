namespace maintenance_buddy_api.api.commands;

public record CreateVehicleCommand
(
    string Name,
    int Kilometer
);