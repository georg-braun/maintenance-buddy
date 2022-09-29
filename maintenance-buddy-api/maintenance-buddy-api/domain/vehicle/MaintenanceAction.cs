namespace maintenance_buddy_api.domain;

public class MaintenanceAction
{
    public int Kilometer { get; internal set; }
    public Guid Id { get; init; }
    
    public Guid ActionTemplateId { get; init; }
    public DateTime Date { get; internal set; }
    public string Note { get; internal set; } = string.Empty;

    public void ChangeKilometer(int kilometer)
    {
        Kilometer = kilometer;
    }

    public void ChangeNote(string note)
    {
        Note = note;
    }
    
    public void ChangeDate(DateTime date)
    {
        Date = date;
    }
}