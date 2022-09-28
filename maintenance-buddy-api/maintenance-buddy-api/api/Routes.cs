namespace maintenance_buddy_api.api;

public static class Routes
{
    private const string Base = "/api";
    private const string Version = "";

    // Commands
    public const string CreateVehicle = $"{Base}{Version}/create-vehicle";
    public const string DeleteVehicle = $"{Base}{Version}/delete-vehicle";
    public const string RenameVehicle = $"{Base}{Version}/rename-vehicle";
    public const string ChangeVehicleKilometer = $"{Base}{Version}/change-vehicle-kilometer";
    public const string AddActionTemplate = $"{Base}{Version}/add-action-template";
    public const string DeleteActionTemplate = $"{Base}{Version}/delete-action-template";
    public const string AddAction = $"{Base}{Version}/add-action";
    public const string DeleteAction = $"{Base}{Version}/delete-action";
    
    // Queries
    public const string Status = $"{Base}{Version}/Status";
    public const string ActionTemplateQuery = $"{Base}{Version}/get-action-templates";
    public const string ActionsQuery = $"{Base}{Version}/get-actions-of-template";
    public const string ActionsOfVehicleQuery = $"{Base}{Version}/get-actions-of-vehicle";
    public const string VehiclesQuery = $"{Base}{Version}/get-vehicles";
}