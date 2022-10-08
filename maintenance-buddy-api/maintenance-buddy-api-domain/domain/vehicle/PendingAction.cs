namespace maintenance_buddy_api.domain;

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