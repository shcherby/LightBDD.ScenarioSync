using System.Collections.ObjectModel;

namespace LightBDD.ScenarioSync.Core.Entities;

public class Relations : ReadOnlyCollection<int>
{
    public Relations(IEnumerable<int> ids)
        : base(ids.ToList())
    {
    }
}