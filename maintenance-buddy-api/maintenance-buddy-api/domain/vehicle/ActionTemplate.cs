namespace maintenance_buddy_api.domain;

public class ActionTemplate
{
    public string Name { get; init; }
    public int KilometerInterval { get; init; }
    public TimeSpan TimeInterval { get; init; }
}