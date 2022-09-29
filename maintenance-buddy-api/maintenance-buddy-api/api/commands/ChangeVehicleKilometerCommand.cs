namespace maintenance_buddy_api.api.commands;

public record ChangeVehicleKilometerCommand
(
    string VehicleId,
    int Kilometer
);