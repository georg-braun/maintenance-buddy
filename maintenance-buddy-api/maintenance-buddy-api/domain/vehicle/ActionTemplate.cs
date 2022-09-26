namespace maintenance_buddy_api.domain;

public class ActionTemplate
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int KilometerInterval { get; init; }
    public TimeSpan TimeInterval { get; init; }
    
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
}