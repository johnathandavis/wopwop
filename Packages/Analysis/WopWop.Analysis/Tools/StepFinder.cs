using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Text;
using WopWop.Analysis.Structure;

namespace WopWop.Analysis.Tree;

internal static class StepFinder
{
    internal static List<MachineStep> GetSteps(MethodDeclarationSyntax m)
    {
        var statements = m.Body!.Statements;
        var src = m.SyntaxTree.GetText();
        int start = m.Body!.Statements[0].FullSpan.Start;
        
        var steps = new List<MachineStep>();
        for (int i = 0; i < statements.Count; i++)
        {
            var st = statements[i];
            var tokens = st.DescendantNodes((node => true)).ToList();
            for (int j = 0; j < tokens.Count(); j++)
            {
                var t = tokens[j];
                if (t is AwaitExpressionSyntax a)
                {
                    var step = CreateStepFromExpression(src, a, ref start);
                    steps.Add(step);
                }
            }
        }

        if (start < m.Body.FullSpan.End)
        {
            var end = m.Body.Statements.Last().FullSpan.End;
            string remainingSource = src.GetSubText(new TextSpan(start, end - start)).ToString();
            steps.Add(new MachineStep(remainingSource, null));
        }

        return steps;
    }

    internal static MachineStep CreateStepFromExpression(SourceText src, AwaitExpressionSyntax a, ref int start)
    {
        var expressionToTrimTo = FindNodeToTrimStepSource(a);
        var trimEnd = expressionToTrimTo.FullSpan.End;
        string sourceCode = src.GetSubText(new TextSpan(start, trimEnd - start)).ToString();
        start = expressionToTrimTo.FullSpan.End;
        var awaitedExpression = GetAwaitedExpressionString(a.Expression);
        return new MachineStep(sourceCode, awaitedExpression);
    }

    internal static SyntaxNode FindNodeToTrimStepSource(AwaitExpressionSyntax a)
    {
        if (a.Parent is ExpressionStatementSyntax || a.Parent is ParenthesizedExpressionSyntax)
        {
            return a.Parent;
        }
        return a;
    }

    internal static string GetAwaitedExpressionString(ExpressionSyntax exp)
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