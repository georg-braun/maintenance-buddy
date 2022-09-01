namespace maintenance_buddy_api.api.commands;

public record CreateVehicleCommand
(
    string Name,
    int Kilometer
);

public record AddActionTemplateCommand
(
    string VehicleId,
    string Name,
    int KilometerInterval,
    TimeSpan TimeInterval
);