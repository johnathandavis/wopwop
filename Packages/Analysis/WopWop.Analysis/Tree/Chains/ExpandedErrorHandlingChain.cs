using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WopWop.Analysis.Tree.Chains;

public class ExpandedErrorHandlingChain
{
    public TryStatementSyntax Try { get; set; }
    public CatchDeclarationSyntax[]? Catches { get; set; }
    public FinallyClauseSyntax? Finally { get; set; }
}