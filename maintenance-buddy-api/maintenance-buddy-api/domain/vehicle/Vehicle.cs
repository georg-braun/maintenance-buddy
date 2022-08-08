namespace maintenance_buddy_api.domain;

public class Vehicle
{
    public string Name { get; init; }
    public int Kilometer { get; init; }
    
    public List<ActionTemplate> ActionTemplates { get; private set; }

    public Vehicle()
    {
        ActionTemplates = new List<ActionTemplate>();
    }

    public void AddActionTemplate(string name, int kilometerInterval, TimeSpan timeInterval)
    {
        var actionTemplate = new ActionTemplate(){
            Id = Guid.NewGuid(),
            Name = name,
            KilometerInterval = kilometerInterval, 
            TimeInterval = timeInterval
        };
        
        ActionTemplates.Add(actionTemplate);
    }

    public IReadOnlyList<ActionTemplate> GetActionTemplates()
    {
        return ActionTemplates;
    }

    public ActionTemplate GetActionTemplate(string actionTemplateName)
    {
        return ActionTemplates.FirstOrDefault(_ => _.Name.Equals(actionTemplateName));
    }

 
}

public static class VehicleFactory
{
    public static Vehicle Create(string name, int kilometer)
    {
        return new Vehicle()
        {
            Name = name,
            Kilometer = kilometer
        };
    }
}