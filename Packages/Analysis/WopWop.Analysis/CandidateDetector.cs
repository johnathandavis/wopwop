using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WopWop.Analysis.Extensions;

namespace WopWop.Analysis;

public class CandidateDetector : ISyntaxReceiver
{
    public Dictionary<ClassDeclarationSyntax, List<MachineCandidate>> AsyncMethods { get; } 
    
    public CandidateDetector()
    {
        AsyncMethods = new Dictionary<ClassDeclarationSyntax, List<MachineCandidate>>();
    }
    
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax classSyntaxNode)
        {
            var classMachineCandidates =
                classSyntaxNode.Members
                    .OfType<MethodDeclarationSyntax>()
                    .Where(m => m.IsAsync() && m.IsCustomTask())
                    .Select(m => new MachineCandidate(classSyntaxNode, m))
                    .ToList();
            
            AsyncMethods.Add(classSyntaxNode, classMachineCandidates);
        }
    }
}