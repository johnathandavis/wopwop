using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using WopWop.Analysis.Structure;
using WopWop.Core.Info;
using WopWop.Core.Manifest;

namespace WopWop.Core.Extensions;

public static class AsyncStateMachineExtensions
{
    private static readonly Regex StateFieldRegex =
        new Regex("<(?<fieldName>[^>]+)>[a-zA-Z0-9_`]+", RegexOptions.Compiled);
    public static StateMachineRuntimeInfo GetRuntimeInfo(this IAsyncStateMachine sm)
    {
        return new StateMachineRuntimeInfo(sm);
    }
    
    public static MachineStructure GetMachineStructure(this IAsyncStateMachine sm) => Extractor.GetStructureFromStateMachine(sm);

    public static Dictionary<string, object> GetVariableState(this IAsyncStateMachine sm)
    {
        var fields = sm.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var stateFields = new Dictionary<string, object>();
        foreach (var f in fields)
        {
            var match = StateFieldRegex.Match(f.Name);
            if (!match.Success || !match.Groups.ContainsKey("fieldName")) continue;
            var fn = match.Groups["fieldName"].Value;
            stateFields.Add(fn, f.GetValue(sm));
        }

        return stateFields;
    }
}