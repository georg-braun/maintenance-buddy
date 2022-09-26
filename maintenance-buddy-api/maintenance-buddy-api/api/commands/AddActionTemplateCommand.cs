namespace maintenance_buddy_api.api.commands;

public record AddActionTemplateCommand
(
    string VehicleId,
    string Name,
    int KilometerInterval,
    TimeSpan TimeInterval
);