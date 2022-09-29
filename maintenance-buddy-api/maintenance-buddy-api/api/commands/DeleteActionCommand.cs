namespace maintenance_buddy_api.api.commands;

public record DeleteActionCommand
(
    string VehicleId,
    string ActionTemplateId,
    string ActionId
);