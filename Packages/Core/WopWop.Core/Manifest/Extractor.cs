using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Caching;

using WopWop.Analysis.Structure;

namespace WopWop.Core.Manifest;

public static class Extractor
{
    private static MemoryCache Cache = MemoryCache.Default;

    public static MachineStructure GetStructureFromStateMachine(IAsyncStateMachine sm)
    {
        var smType = sm.GetType();
        var assembly = smType.Assembly;
        if (!Cache.Contains(assembly.FullName))
        {
            Cache.Add(assembly.FullName, ExtractManifestForAssembly(assembly), new CacheItemPolicy());
        }
        
        var manifest = (IMachineManifestLookup)Cache.Get(assembly.FullName);
        return manifest.LookupMachineStructure(smType);
    }

    public static IMachineManifestLookup GetLookupFromAssembly(Assembly assembly)
    {
        if (!Cache.Contains(assembly.FullName))
        {
            Cache.Add(assembly.FullName, ExtractManifestForAssembly(assembly), new CacheItemPolicy());
        }
        return (IMachineManifestLookup)Cache.Get(assembly.FullName);
    }
    
    internal static IMachineManifestLookup ExtractManifestForAssembly(Assembly assembly)
    {
        var attribute = assembly.GetCustomAttributes()
            .First(att => att.GetType().Name.StartsWith("WopWopMachineManifestAttribute"));
        var manifestType = attribute.GetType().GetGenericArguments()[0];
        var manifest = (IAssemblyMachineManifest)Activator.CreateInstance(manifestType);
        return new ReadOnlyMachineManifestLookup(manifest);
    }
}