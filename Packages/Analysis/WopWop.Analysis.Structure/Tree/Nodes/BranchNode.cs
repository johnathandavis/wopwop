namespace WopWop.Analysis.Structure.Tree.Nodes;

using WopWop.Analysis.Structure.Tree.Info;

public class BranchNode : TreeNode
{
    public string Alignment { get; set; }
    public BranchType Type { get; set; }
    public List<Branch> Branches { get; set; }
}