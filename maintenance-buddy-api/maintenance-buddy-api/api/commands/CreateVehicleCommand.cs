namespace maintenance_buddy_api.api.commands;

public record CreateVehicleCommand
(
    string Name,
    int Kilometer
);

public record RenameVehicleCommand
(
    string VehicleId,
    string Name
);

public record ChangeVehicleKilometerCommand
(
    string VehicleId,
    int Kilometer
);