using maintenance_buddy_api.domain;

namespace maintenance_buddy_api.api.dto;

public record MaintenanceActionDto
{
    public int Kilometer { get; init; }
    public Guid Id { get; init; }
    public Guid ActionTemplateId { get; init; }
    public DateTime Date { get; init; }
    public string Note { get; init; } = string.Empty;
}

public static class ActionDtoMapper
{
    public static MaintenanceActionDto ToDto(MaintenanceAction action)
    {
        
        return new MaintenanceActionDto()
        {
            Id = action.Id,
            ActionTemplateId = action.ActionTemplateId,
            Date = action.Date,
            Note = action.Note
        };
    }
}