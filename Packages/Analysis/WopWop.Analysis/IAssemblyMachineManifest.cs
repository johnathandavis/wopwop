namespace WopWop.Analysis;

using WopWop.Analysis.Structure;

public interface IAssemblyMachineManifest
{
    public IReadOnlyList<MachineStructure> Machines { get; }
}