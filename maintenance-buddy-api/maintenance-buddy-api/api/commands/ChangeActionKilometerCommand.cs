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