namespace maintenance_buddy_api.api.commands;

public record ChangeActionKilometerCommand
(
    string VehicleId,
    string ActionTemplateId,
    string ActionId,
    int Kilometer
);