using System.ComponentModel.DataAnnotations;

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

     public void AddAction(Guid id, int kilometer, DateTime date)
     {
         var action = new Action()
         {
             Id = Guid.NewGuid(),
             Kilometer = kilometer,
             Date = date
         };
    
         Actions.Add(action);
     }

     public IEnumerable<Action> GetActions()
     {
         return Actions;
     }
}