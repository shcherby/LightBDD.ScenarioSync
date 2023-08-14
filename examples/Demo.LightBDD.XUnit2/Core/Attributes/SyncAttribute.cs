using System;
using System.Collections.Generic;
using System.Linq;
using LightBDD.Framework;
using LightBDD.XUnit2;
using Xunit.Sdk;

namespace Demo.LightBDD.XUnit2.Core.Attributes;

public class SyncAttribute : LabelAttribute
{
    private static readonly IDictionary<string, Type> FeatureNameClassFullNameMap =
        AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(ass => ass.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(FeatureFixture)))
            .ToDictionary(t => t.Name, t => t);

    public SyncAttribute(string featureClassName, string scenarioMethodName)
        : base(GetSyncLabel(featureClassName, scenarioMethodName))
    {
    }

    private static string GetSyncLabel(string featureClassName, string scenarioMethodName)
    {
        if (!FeatureNameClassFullNameMap.TryGetValue(featureClassName, out Type? featureClassType))
        {
            throw new NullException($"Feature class '{featureClassName}' not found in the assembly.");
        }

        List<string> methods = featureClassType.GetMethods().Select(m => m.Name).ToList();
        if (!methods.Contains(scenarioMethodName))
        {
            throw new NullException($"Scenario method name '{scenarioMethodName}' not found in the class '{featureClassName}'.");
        }

        string automatedTestStorage = featureClassType.Assembly.ManifestModule.Name;

        return $"Sync:{automatedTestStorage};{featureClassType.FullName}.{scenarioMethodName}";
    }
}