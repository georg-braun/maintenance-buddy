namespace maintenance_buddy_api.domain;

public class Vehicle
{
    public Guid Id { get; init; }
    public string Name { get; private set; } = string.Empty;
    public int Kilometer { get; private set; }

    public ICollection<ActionTemplate> ActionTemplates { get;  init; }

    private Vehicle()
    {
        ActionTemplates = new List<ActionTemplate>();
    }

    public static Vehicle Create(Guid id, string name, int kilometer)
    {
        return new Vehicle()
        {
            Id = id,
            Name = name,
            Kilometer = kilometer
        };
    }

    public ActionTemplate AddActionTemplate(string name, int kilometerInterval, TimeSpan timeInterval)
    {
        var actionTemplate = ActionTemplate.Create(
            Guid.NewGuid(),
            name,
            kilometerInterval,
            timeInterval);
        
        ActionTemplates.Add(actionTemplate);
        return actionTemplate;
    }

    public ICollection<ActionTemplate> GetActionTemplates()
    {
        return ActionTemplates;
    }

    public ActionTemplate? GetActionTemplate(string actionTemplateName)
    {
        return ActionTemplates.FirstOrDefault(_ => _.Name.Equals(actionTemplateName));
    }


    public void RemoveActionTemplate(Guid actionTemplateId)
    {
        var actionTemplate = ActionTemplates.FirstOrDefault(_ => _.Id.Equals(actionTemplateId));

        if (actionTemplate is null)
            return;
        ActionTemplates.Remove(actionTemplate);
    }

    public MaintenanceAction? AddAction(Guid actionTemplateId, DateTime date, int kilometer, string note)
    {
        var actionTemplate = ActionTemplates.FirstOrDefault(_ => _.Id.Equals(actionTemplateId));
        if (actionTemplate is null)
            return null;
        
        return actionTemplate.AddAction(kilometer, date, note);
    }

    public IEnumerable<MaintenanceAction> GetActions(Guid actionTemplateId)
    {
        return ActionTemplates.FirstOrDefault(_ => _.Id.Equals(actionTemplateId))?.Actions ?? Enumerable.Empty<MaintenanceAction>();
    }

    public IEnumerable<MaintenanceAction> GetActions()
    {
        return ActionTemplates.SelectMany(_ => _.Actions);
    }
    
    
    public void DeleteAction(Guid actionTemplateId, Guid actionId)
    {
        var actionTemplate = GetAction(actionTemplateId);
        if (actionTemplate is null)
            return;

        actionTemplate.DeleteAction(actionId);
    }

    private ActionTemplate? GetAction(Guid actionTemplateId)
    {
        return ActionTemplates.FirstOrDefault(_ => _.Id.Equals(actionTemplateId));
    }

    public void Rename(string name)
    {
        Name = name;
    }

    public void ChangeKilometer(int kilometer)
    {
        Kilometer = kilometer;
    }

    public void ChangeActionKilometer(Guid actionTemplateId, Guid actionId, int kilometer)
    {
        var actionTemplate = GetActionTemplate(actionTemplateId);
        actionTemplate?.ChangeActionKilometer(actionId, kilometer);
    }

    private ActionTemplate? GetActionTemplate(Guid actionTemplateId)
    {
        var actionTemplate = ActionTemplates.FirstOrDefault(_ => _.Id.Equals(actionTemplateId));
        return actionTemplate;
    }
    
    public void ChangeActionNote(Guid actionTemplateId, Guid actionId, string note)
    {
        var actionTemplate = GetActionTemplate(actionTemplateId);
        actionTemplate?.ChangeActionNote(actionId, note);
    }
    
    public void ChangeActionDate(Guid actionTemplateId, Guid actionId, DateTime date)
    {
        var actionTemplate = GetActionTemplate(actionTemplateId);
        actionTemplate?.ChangeActionDate(actionId, date);
    }

    public void ChangeActionTemplateKilometerInterval(Guid actionTemplateId, int kilometerInterval)
    {
        GetActionTemplate(actionTemplateId)?.ChangeKilometerInterval(kilometerInterval);
    }

    public void ChangeActionTemplateTimeInterval(Guid actionTemplateId, TimeSpan timeInterval)
    {
        GetActionTemplate(actionTemplateId)?.ChangeTimeInterval(timeInterval);
    }

    public void ChangeActionTemplateName(Guid actionTemplateId, string name)
    {
        GetActionTemplate(actionTemplateId)?.ChangeName(name);
    }

    public IEnumerable<PendingAction> GetPendingActions(DateTime checkDate)
    {
        return ActionTemplates.Select(_ => _.GetPendingAction(checkDate, Kilometer));
    }
}

public static class VehicleFactory
{
    public static Vehicle Create(string name, int kilometer)
    {
        return Vehicle.Create(Guid.NewGuid(), name, kilometer);
    }
}