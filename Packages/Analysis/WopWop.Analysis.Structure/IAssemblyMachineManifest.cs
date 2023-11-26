namespace WopWop.Analysis.Structure;

using WopWop.Analysis.Structure;

public interface IAssemblyMachineManifest
{
    public IReadOnlyList<MachineStructure> Machines { get; }
}