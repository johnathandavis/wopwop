using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WopWop.Analysis.Extensions;

public static class SyntaxExtensions
{
    private static Regex TaskNamePattern = new Regex("RemoteTask<[^]]+>");
    public static bool IsAsync(this MethodDeclarationSyntax m) =>
        m.Modifiers.Any((st) => st.IsKind(SyntaxKind.AsyncKeyword));
    
    public static bool IsCustomTask(this MethodDeclarationSyntax m) =>
        TaskNamePattern.IsMatch(m.ReturnType.ToString());
   
    public static List<MachineCandidate> FindCandidates(this ClassDeclarationSyntax c) =>
        c.Members
            .OfType<MethodDeclarationSyntax>()
            .Where(m => m.IsAsync() && m.IsCustomTask())
            .Select(m => new MachineCandidate(c, m))
            .ToList();
}