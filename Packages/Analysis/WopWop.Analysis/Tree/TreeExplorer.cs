namespace WopWop.Analysis.Tree;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using WopWop.Analysis.Structure.Tree;

public class TreeExplorer : ITreeExplorer
{
    internal static MethodTree ExploreMethodTree(MethodDeclarationSyntax syntax)
    {
        var mt = new MethodTree();
        
        // What goes in a method? Statements, of course:
        
        foreach (var stmt in syntax.Body.Statements)
        {
            if (stmt is IfStatementSyntax)
            {
                
            }
        }
    }
}