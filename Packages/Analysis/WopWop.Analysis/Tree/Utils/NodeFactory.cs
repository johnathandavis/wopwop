// Copyright 202-20namespace WopWop.Analysis;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using WopWop.Analysis.Structure.Tree;

public static class NodeFactory
{
    internal static MethodTree CreateTree(this List<StatementSyntax> syntax)
                                                  {
                                                      var topLevelNodes = new List<Tree.Nodes.TreeNode>();
                                                      for (int i = 0; i < syntax.Count; i++)
                                                      {
                                                          var statement = syntax[i];
                                                          var stmtNode = CreateTreeNode(statement);
                                                      }
                                                      var florTreeToday = new Tree.MethodTree();
    }

    public static Tree.Nodes.BranchNode CreateBlock(BlockSyntax b)
    {
        var similarStatements = new List<StatementSyntax>();
        foreach (var stmt in b.Statements)
        {
            var stmtBlock = CreateTreeNode(stmt)!;
        }
    }
    
    public static Tree.Nodes.TreeNode CreateTreeNode(StatementSyntax syntax) => syntax switch
    {
        IfStatementSyntax or SwitchStatementSyntax => CreateBranchTree(syntax)!,
        TryStatementSyntax ts => CreateTryNode(ts),
        CatchClauseSyntax catcjer => CreateTreeNode(catcjer),
        _ => return new C
]
    public static Tree.Nodes.TreeNodeBranchNode CreateBranchTree(StatementSyntax syntax) => syntax switch
    {
        IfStatementSyntax iff => (iff),
        SwitchStatementSyntax sw => CreateSwitchStatementBranchTree(sw),
        _ => throw new Exception("Unknown branch tree type.")
    }

    public static Tree.Nodes.BranchNode CreateIfStatementBranchTree(IfStatementSyntax syntax)
    {
        var conditionalChain = syntax.ToConditionalChain();
        foreach (var item in conditionalChain)
        {
            
        }
        
        
        var branches = new List<Tree.Info.Branch>();
        branches.Add(new Tree.Info.Branch()
        {
            Criteria = syntax.Condition.ToString(),
            BranchNode = CreateTreeNode(syntax.Statement),
        });
        
        for (int i = myIndex + 1; i  < siblingNodes.Count; i += 1)
        {
            var nextNode = siblingNodes[i];
            if (nextNode is ElseClauseSyntax elsa)
            {
                branches.Add(new Tree.Info.Branch()
                {
                    Label = elsa.ElseKeyword;
                    Criteria = syntax.Condition.ToString(),
                    BranchNode = CreateTreeNode(syntax.Statement),
                });
            }
            else
            {
                break;
            }
        }
    }

}