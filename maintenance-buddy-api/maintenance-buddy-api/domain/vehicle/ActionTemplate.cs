namespace maintenance_buddy_api.domain;

public class ActionTemplate
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public int KilometerInterval { get; init; }
    public TimeSpan TimeInterval { get; init; }
    
     public List<Action> Actions { get; }

     public ActionTemplate()
     {
          Actions = new List<Action>();
     }

     public Action AddAction(int kilometer, DateTime date, string note)
     {
         var action = new Action()
         {
             Id = Guid.NewGuid(),
             Kilometer = kilometer,
             Date = date,
             Note = note 
         };
    
         Actions.Add(action);
         return action;
     }

     public IEnumerable<Action> GetActions()
     {
         return Actions;
     }

     public Action? GetAction(Guid actionId)
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