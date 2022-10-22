namespace maintenance_buddy_api.api.commands;

public record ChangeActionKilometerCommand
(
    string VehicleId,
    string ActionTemplateId,
    string ActionId,
    int Kilometer
);

public record ChangeActionNoteCommand
(
    string VehicleId,
    string ActionTemplateId,
    string ActionId,
    string Note
);

public record ChangeActionDateCommand
(
    string VehicleId,
    string ActionTemplateId,
    string ActionId,
    DateTime Date
);

public record ChangeActionTemplateNameCommand
(
    string VehicleId,
    string ActionTemplateId,
    string Name
);

public record ChangeActionTemplateKilometerIntervalCommand
(
    string VehicleId,
    string ActionTemplateId,
    int KilometerInterval
);
public record ChangeActionTemplateTimeIntervalCommand
(
    string VehicleId,
    string ActionTemplateId,
    int TimeIntervalInDays
);