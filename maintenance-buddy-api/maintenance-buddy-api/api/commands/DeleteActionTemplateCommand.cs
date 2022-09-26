namespace maintenance_buddy_api.api.commands;

public record DeleteActionTemplateCommand
(
    string VehicleId,
    string ActionTemplateId
);