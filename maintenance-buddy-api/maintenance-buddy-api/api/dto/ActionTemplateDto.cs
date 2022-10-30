using maintenance_buddy_api.domain;

namespace maintenance_buddy_api.api.dto;

public record ActionTemplateDto
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    
    public int KilometerInterval { get; init; } 
    
    public int TimeIntervalInDays { get; init; } 
}
public static class ActionTemplateDtoMapper
{
    public static ActionTemplateDto ToDto(ActionTemplate template)
    {
        return new ActionTemplateDto
        {
            Id = template.Id.ToString(),
            Name = template.Name,
            KilometerInterval = template.KilometerInterval,
            TimeIntervalInDays = template.TimeInterval.Days
        };
    }
}