namespace maintenance_buddy_api.infrastructure.database_model;

/// <summary>
///     This class is the connection between a user and the available vehicle
/// </summary>
public class UserVehicleConnection
{
    public UserVehicleConnection(string nameIdentifier, Guid vehicleId)
    {
        NameIdentifier = nameIdentifier;
        VehicleId = vehicleId;
    }

    public string NameIdentifier { get; init; }
    public Guid VehicleId { get; init; }
    
}