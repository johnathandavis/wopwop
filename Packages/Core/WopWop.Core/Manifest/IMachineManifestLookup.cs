using WopWop.Analysis.Structure;

namespace WopWop.Core.Manifest;

public interface IMachineManifestLookup
{
    public MachineStructure LookupMachineStructure(Type t);
}