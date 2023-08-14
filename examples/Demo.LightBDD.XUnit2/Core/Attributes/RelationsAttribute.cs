using System.Collections.Generic;
using LightBDD.Framework;

namespace Demo.LightBDD.XUnit2.Core.Attributes;

public class RelationsAttribute : LabelAttribute
{
    public RelationsAttribute(params int[] relationsId)
        : base(GetLabel(relationsId))
    {
    }

    private static string GetLabel(IEnumerable<int> relationsId) => $"Relations:{string.Join(",", relationsId)}";
}