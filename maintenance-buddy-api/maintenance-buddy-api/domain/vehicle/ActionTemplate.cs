namespace maintenance_buddy_api.domain;

public class ActionTemplate
{
    public static ActionTemplate Create(Guid id, string name, int kilometerInterval, TimeSpan timeInterval)
    {
        return new ActionTemplate()
        {
            Id = id,
            Name = name,
            KilometerInterval = kilometerInterval,
            TimeInterval = timeInterval,
        };

    }

    private ActionTemplate()
    {
        Actions = new List<MaintenanceAction>();
    }

    public Guid Id { get; init; }
    public string Name { get; internal set; } = string.Empty;
    public int KilometerInterval { get; internal set; }
    public TimeSpan TimeInterval { get; internal set; }

    public List<MaintenanceAction> Actions { get; internal set; } = null!;

    public MaintenanceAction AddAction(int kilometer, DateTime date, string note)
    {
        var action = MaintenanceAction.Create(
            Guid.NewGuid(),
            Id,
            kilometer,
            date,
            note
        );

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

    public PendingAction GetPendingAction(DateTime checkDate, int vehicleKilometer)
    {
        
        TimeSpan timespanTillAction = new TimeSpan();
        var kilometerTillAction = 0;
        
        if (Actions.Count > 0)
        {
            // assume that the kilometer doesn't decrease with time :> (no engine change)
            var latestAction = Actions.MaxBy(_ => _.Date)!;
            timespanTillAction = latestAction.Date.Add(TimeInterval) - checkDate;
            kilometerTillAction = latestAction.Kilometer + KilometerInterval - vehicleKilometer;
        }
        
        return PendingAction.Create(Id, timespanTillAction, kilometerTillAction);
    }
}

public class PendingAction
{
    public Guid ActionTemplateId { get; init; }
    public TimeSpan TimeTillAction { get; init; }
    public int KilometerTillAction { get; init; }
    public bool Exceeded => TimeTillAction.Ticks < 0 || KilometerTillAction < 0;

    private PendingAction(){}

    public static PendingAction Create(Guid actionTemplateId, TimeSpan timeTillAction, int kilometer)
    {
        return new PendingAction
        {
            ActionTemplateId = actionTemplateId,
            TimeTillAction = timeTillAction,
            KilometerTillAction = kilometer
        };
    }
}