using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WopWop.Analysis;

public class MachineCandidate
{
    public MachineCandidate(ClassDeclarationSyntax classSyntax, MethodDeclarationSyntax methodSyntax)
    {
        this.ClassSyntax = classSyntax;
        this.MethodSyntax = methodSyntax;
    }
    
    public ClassDeclarationSyntax ClassSyntax { get; private set; }
    public MethodDeclarationSyntax MethodSyntax { get; private set; }
}