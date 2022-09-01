namespace maintenance_buddy_api.api;

public static class Routes
{
    private const string Base = "/api";
    private const string Version = "";

    public const string Status = $"{Base}{Version}/Status";
    public const string CreateVehicle = $"{Base}{Version}/create-vehicle";
    public const string AddActionTemplate = $"{Base}{Version}/add-action-template";
}