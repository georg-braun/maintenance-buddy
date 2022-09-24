namespace maintenance_buddy_api.domain;

public class MaintenanceAction
{
    public int Kilometer { get; init; }
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
    public string Note { get; init; } = string.Empty;
}