namespace WopWop.Analysis.Tree.Chains;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using WopWop.Analysis.Structure.Tree.Nodes;

public class ConditionalChainExpansion
{
    internal static BranchNode CreateNodeFromConditional(IfStatementSyntax syntax)
    {
        var chain = UnrollChain(syntax);
        return 
    }

    internal static  ExpandedConditionalChain UnrollChain(IfStatementSyntax syntax)
    {
        var allSiblings = syntax.Parent.ChildNodes().ToList();
        var start = allSiblings.IndexOf(syntax);
        var siblingNodes = allSiblings
            .Skip(start).ToList();
        var expanded = new ExpandedConditionalChain();
        expanded.If = syntax;
        for (int i = 0; i < siblingNodes.Count; i += 2)
        {
            var el = siblingNodes[i];
            if (el is not ElseClauseSyntax)
            {
                throw new ArgumentException("Consecutiveifs?");
            }

            if (i + 1 > siblingNodes.Count)
            {
                // We're done
                expanded.Else = (ElseClauseSyntax)el;
                return expanded;
            }
            
            var el2 = siblingNodes[i+1];
            if (el2 is not IfStatementSyntax)
            {
                // THis is a new clause, don;t include it
                expanded.Else = (ElseClauseSyntax)el;
                return expanded;
            }
            
            var ifs = (IfStatementSyntax)el2;
            expanded.ElseIfs.Add(ifs);
        }

        return expanded;
    }
}