namespace maintenance_buddy_api.domain;

public class Vehicle
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public int Kilometer { get; init; }

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
        var actionTemplate = new ActionTemplate(){
            Id = Guid.NewGuid(),
            Name = name,
            KilometerInterval = kilometerInterval,
            TimeInterval = timeInterval
        };
        
        ActionTemplates.Add(actionTemplate);
        return actionTemplate;
    }

    public ICollection<ActionTemplate> GetActionTemplates()
    {
        return ActionTemplates;
    }

    public ActionTemplate GetActionTemplate(string actionTemplateName)
    {
        return ActionTemplates.FirstOrDefault(_ => _.Name.Equals(actionTemplateName));
    }


    public void RemoveActionTemplate(Guid actionTemplateId)
    {
        var actionTemplate = ActionTemplates.FirstOrDefault(_ => _.Id.Equals(actionTemplateId));
        ActionTemplates.Remove(actionTemplate);
    }

    public Action? AddAction(Guid actionTemplateId, DateTime date, int kilometer)
    {
        var actionTemplate = ActionTemplates.FirstOrDefault(_ => _.Id.Equals(actionTemplateId));
        if (actionTemplate is null)
            return null;
        
        return actionTemplate.AddAction(kilometer, date);
    }

    public IEnumerable<Action> GetActions(Guid actionTemplateId)
    {
        return ActionTemplates.FirstOrDefault(_ => _.Id.Equals(actionTemplateId))?.Actions ?? Enumerable.Empty<Action>();
    }
}

public static class VehicleFactory
{
    public static Vehicle Create(string name, int kilometer)
    {
        return Vehicle.Create(Guid.NewGuid(), name, kilometer);
    }
}