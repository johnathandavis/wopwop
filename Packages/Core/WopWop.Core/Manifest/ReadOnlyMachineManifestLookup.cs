using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using WopWop.Analysis.Structure;

namespace WopWop.Core.Manifest;
using WopWop.Analysis.Structure;

public class ReadOnlyMachineManifestLookup : IMachineManifestLookup
{
    private static readonly Regex MachineNamePattern =
        new Regex("(?<type>[^+]+)+.<(?<name>[^>]+)>", RegexOptions.Compiled);
    private readonly IReadOnlyDictionary<Type, MachineStructure> typeMachineStructures;

    public ReadOnlyMachineManifestLookup(IAssemblyMachineManifest assemblyMachineManifest)
    {
        var assembly = assemblyMachineManifest.GetType().Assembly;
        var mcs = new Dictionary<Type, MachineStructure>();

        var typeMethodDict = new Dictionary<string, Dictionary<string, Type>>();

        foreach (var mc in assemblyMachineManifest.Machines)
        {
            if (!typeMethodDict.ContainsKey(mc.Id.TypeName))
            {
                var mcType = assembly.GetType(mc.Id.TypeName);
                var typeDict = mcType.GetMethods()
                    .Select(m => (m, m.GetCustomAttribute<AsyncStateMachineAttribute>()))
                    .Where(k => k.Item2 != null)
                    .ToDictionary(
                        k => k.m.Name,
                        k => k.Item2.StateMachineType);
                typeMethodDict.Add(mc.Id.TypeName, typeDict);
            }

            var smType = typeMethodDict[mc.Id.TypeName][mc.Id.MethodName];
            
            mcs.Add(smType, mc);
        }

        this.typeMachineStructures = mcs;
    }
    
    internal IReadOnlyDictionary<Type, MachineStructure> MachineStructures => typeMachineStructures;

    public MachineStructure LookupMachineStructure(Type t)
    {
        var lookupT = t.IsGenericType ? t.GetGenericTypeDefinition() : t;
        return MachineStructures[lookupT];
    }
}