using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WopWop.Analysis.Structure.Tree;

namespace WopWop.Analysis.Tree;

public static class TreeExtensions
{
    
    public static TreeNode CreateTreeNode(StatementSyntax syntax) => syntax switch
    {
        IfStatementSyntax or SwitchStatementSyntax => CreateBranchTree(syntax),
        TryStatementSyntax ts => CreateTryNode(ts),
        BlockSyntax b =>-_
        CatchClauseSyntax catcjer => 
    }

    public static TriedNode CreateTryNode(TryStatementSyntax ts)
    {
        new TriedNode()
        {
            
        }
    }

    public static BranchNode CreateBranchTree(StatementSyntax syntax) => syntax switch
    {
        IfStatementSyntax iff => CreateIfStatementBranchTree(iff),
        SwitchStatementSyntax sw => CreateSwitchStatementBranchTree(sw),
        _ => throw new Exception("Unknown branch tree type.")
    }

    public static BranchNode CreateIfStatementBranchTree(IfStatementSyntax syntax)
    {
        var deviations = new List<BranchDeviation>();
        deviations.Add(new BranchDeviation(syntax));
        if (syntax.Else != null)
        {
            deviations.Add( BranchDeviationFactory.FromExpression(syntax.Else));
        }
    }

    public static BranchNode CreateSwitchStatementBranchTree(SwitchStatementSyntax syntax)
    {
        var deviations = new List<BranchDeviation>();
        foreach (var s in syntax.Sections)
        {
            deviations.Add(new BranchDeviation(syntax, s));
        }
    }
    
    public static BranchNode CreateSwitchExpressionBranchTree(SwitchExpressionSyntax syntax)
    {
        var deviations = new List<BranchDeviation>();
        foreach (var s in syntax.Arms)
        {
            deviations.Add(new BranchDeviation(syntax.GoverningExpression, s));
        }
    }
}