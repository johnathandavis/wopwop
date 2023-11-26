using WopWop.Analysis.Structure.Tree;

namespace WopWop.Analysis.Structure.Builder;

public class MachineStructureBuilder
{
    private MachineIdentifier id;
    private MethodTree tree;

    public MachineStructureBuilder WithId(MachineIdentifier id)
    {
        this.id = id;
        return this;
    }

    public MachineStructureBuilder WithTree(MethodTree tree)
    {
        this.tree = tree;
        return this;
    }

    public MachineStructure Build()
    {
        return new MachineStructure(this.id, this.tree);
    }
}