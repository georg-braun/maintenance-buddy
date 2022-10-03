namespace maintenance_buddy_api.api;

public static class Routes
{
    private const string Base = "/api";
    private const string Version = "";

    // vehicle
    public const string CreateVehicle = $"{Base}{Version}/vehicle/create";
    public const string DeleteVehicle = $"{Base}{Version}/vehicle/delete";
    public const string RenameVehicle = $"{Base}{Version}/vehicle/rename";
    public const string ChangeVehicleKilometer = $"{Base}{Version}/vehicle/change-kilometer";
    public const string VehiclesQuery = $"{Base}{Version}/vehicle";
    public const string VehiclePendingActions = $"{Base}{Version}/vehicle/pending-actions";
    
    // action template
    public const string AddActionTemplate = $"{Base}{Version}/action-template/create";
    public const string DeleteActionTemplate = $"{Base}{Version}/action-template/delete";
    public const string ChangeActionTemplateKilometerInterval = $"{Base}{Version}/action-template/change-kilometer-interval";
    public const string ChangeActionTemplateTimeInterval = $"{Base}{Version}/action-template/change-time-interval";
    public const string ChangeActionTemplateName = $"{Base}{Version}/action-template/rename";
    public const string ActionTemplateQuery = $"{Base}{Version}/action-template";
    
    
    // action
    public const string AddAction = $"{Base}{Version}/action/create";
    public const string DeleteAction = $"{Base}{Version}/action/delete";
    public const string ChangeActionKilometer = $"{Base}{Version}/action/change-kilometer";
    public const string ChangeActionNote = $"{Base}{Version}/action/change-note";
    public const string ChangeActionDate = $"{Base}{Version}/action/change-date";
    public const string ActionsByTemplate = $"{Base}{Version}/action/by-action-template";
    public const string ActionsByVehicle = $"{Base}{Version}/action/by-vehicle";
    
    
    // status
    public const string Status = $"{Base}{Version}/Status";
}