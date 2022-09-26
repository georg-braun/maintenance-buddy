namespace maintenance_buddy_api.api.commands;

public record AddActionCommand
(
    string VehicleId,
    string ActionTemplateId,
    DateTime Date,
    int Kilometer,
    string Note
);
public record DeleteActionCommand
(
    string VehicleId,
    string ActionTemplateId,
    string ActionId
);