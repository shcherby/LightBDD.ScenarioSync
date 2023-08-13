using System.Collections.ObjectModel;

namespace LightBDD.ScenarioSync.Core.Entities;

public class Tags : ReadOnlyCollection<string>
{
    public Tags(IList<string> list) : base(list)
    {
    }
}