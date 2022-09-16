namespace maintenance_buddy_api.api;

public static class Routes
{
    private const string Base = "/api";
    private const string Version = "";

    // Commands
    public const string CreateVehicle = $"{Base}{Version}/create-vehicle";
    public const string AddActionTemplate = $"{Base}{Version}/add-action-template";
    public const string DeleteActionTemplate = $"{Base}{Version}/delete-action-template";
    public const string AddAction = $"{Base}{Version}/add-action";
    public const string DeleteAction = $"{Base}{Version}/delete-action";
    
    // Queries
    public const string Status = $"{Base}{Version}/Status";
    public const string ActionTemplateQuery = $"{Base}{Version}/get-action-templates";
    public const string ActionsQuery = $"{Base}{Version}/get-actions";
    public const string VehiclesQuery = $"{Base}{Version}/get-vehicles";
}