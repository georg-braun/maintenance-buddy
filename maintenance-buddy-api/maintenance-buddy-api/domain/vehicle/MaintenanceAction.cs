namespace maintenance_buddy_api.domain;

public class MaintenanceAction
{
    public int Kilometer { get; internal set; }
    public Guid Id { get; init; }
    
    public Guid ActionTemplateId { get; init; }
    public DateTime Date { get; init; }
    public string Note { get; init; } = string.Empty;

    public void ChangeKilometer(int kilometer)
    {
        Kilometer = kilometer;
    }
}