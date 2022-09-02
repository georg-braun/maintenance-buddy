namespace maintenance_buddy_api.api;

public static class Routes
{
    private const string Base = "/api";
    private const string Version = "";

    // Commands
    public const string CreateVehicle = $"{Base}{Version}/create-vehicle";
    public const string AddActionTemplate = $"{Base}{Version}/add-action-template";
    public const string DeleteActionTemplate = $"{Base}{Version}/delete-action-template";
    
    // Queries
    public const string Status = $"{Base}{Version}/Status";
    public const string ActionTemplateQuery = $"{Base}{Version}/get-action-templates";
}