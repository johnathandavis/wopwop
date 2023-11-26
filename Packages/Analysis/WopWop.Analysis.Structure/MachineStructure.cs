using WopWop.Analysis.Structure.Tree;

namespace WopWop.Analysis.Structure;

public class MachineStructure
{
    public MachineStructure(MachineIdentifier id, MethodTree tree)
    {
        this.Id = id;
        this.Tree = tree;
    }
    
    public MachineIdentifier Id { get; private set; }
    public MethodTree Tree { get; set; }
}