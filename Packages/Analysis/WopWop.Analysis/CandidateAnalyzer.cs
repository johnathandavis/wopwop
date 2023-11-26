namespace WopWop.Analysis;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WopWop.Analysis.Structure;

public static class CandidateAnalyzer
{
    public static bool TryGetMachineStructure(EnrichedMachineCandidate candidate,
        out MachineStructure mcStructure)
    {
        var methodId = candidate.MethodSyntax.Identifier.Text;
        var sym = candidate.Model.GetDeclaredSymbol(candidate.ClassSyntax) as ITypeSymbol;

        string returnType = candidate.MethodSyntax.ReturnType.ToString();
        string sig = candidate.MethodSyntax.ParameterList.ToString();
        var steps = StepFinder.GetSteps(candidate.MethodSyntax);
        var id = new MachineIdentifier(sym.ToString(), methodId, $"{returnType}{sig}");
        mcStructure = new MCStructure(id, steps);
        return true;
    }

    private static string GetAwaitedExpressionString(ExpressionSyntax exp, SemanticModel model, DataFlowAnalysis flow)
    {
        if (exp.IsKind(SyntaxKind.InvocationExpression))
        {
            return exp.ToString();
        }
        else if (exp.IsKind(SyntaxKind.IdentifierName))
        {
            return exp.ToString();
        }
        else
        {
            return exp.ToString();
        }
    }
}