namespace maintenance_buddy_api.domain;

public class ActionTemplate
{
    public Guid Id { get; init; }
    public string Name { get; internal  set; } = string.Empty;
    public int KilometerInterval { get; internal set; }
    public TimeSpan TimeInterval { get; internal set; }

    public List<MaintenanceAction> Actions { get; }

    public ActionTemplate()
    {
        Actions = new List<MaintenanceAction>();
    }

    public MaintenanceAction AddAction(int kilometer, DateTime date, string note)
    {
        var action = new MaintenanceAction()
        {
            Id = Guid.NewGuid(),
            ActionTemplateId = Id,
            Kilometer = kilometer,
            Date = date,
            Note = note
        };

        Actions.Add(action);
        return action;
    }

    public IEnumerable<MaintenanceAction> GetActions()
    {
        return Actions;
    }

    public MaintenanceAction? GetAction(Guid actionId)
    {
        return Actions.FirstOrDefault(_ => _.Id.Equals(actionId));
    }

    public void DeleteAction(Guid actionId)
    {
        var action = GetAction(actionId);

        if (action is null)
            return;

        Actions.Remove(action);
    }

    public void ChangeActionKilometer(Guid actionId, int kilometer)
    {
        GetAction(actionId)?.ChangeKilometer(kilometer);
    }

    public void ChangeActionNote(Guid actionId, string note)
    {
        GetAction(actionId)?.ChangeNote(note);
    }
    
    public void ChangeActionDate(Guid actionId, DateTime date)
    {
        GetAction(actionId)?.ChangeDate(date);
    }


    public void ChangeKilometerInterval(int kilometerInterval)
    {
        KilometerInterval = kilometerInterval;
    }

    public void ChangeTimeInterval(TimeSpan timeInterval)
    {
        TimeInterval = timeInterval;
    }

    public void ChangeName(string name)
    {
        Name = name;
    }
}