using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WopWop.Analysis.Tree.Chains;

public class ExpandedConditionalChain
{
    public IfStatementSyntax If { get; set; }
    public List<IfStatementSyntax> ElseIfs { get; set; } = new List<IfStatementSyntax>();
    public ElseClauseSyntax? Else { get; set; }
}