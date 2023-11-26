
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using WopWop.Analysis.Structure;

public class AstExplorerTests
{
    [Fact]
    public void AnaylzeAST()
    {
        string src = $$"""
                       try
                       {
                          int ans = int.Parse("-3.14");
                          Console.WriteLine(ans);
                       }
                       catch (ArgumentException arg)
                       {
                          Console.WriteLine("I'm so stupid.");
                          throw arg;
                       }
                       catch
                       {
                        throw;
                       }
                       finally
                       {
                        Console.WriteLine("Cleanup!");
                       }
                       
                       """;
        GetTree(src);
    }

    private MethodDeclarationSyntax GetTree(string txt)
    {
        string surroundedSource = $$"""
                                    public class Program
                                    {
                                        public async Task<string> RunAsync()
                                        {
                                             {{txt}}
                                        }
                                    }
                                    """;
        var tree = CSharpSyntaxTree.ParseText(surroundedSource).GetRoot() as CompilationUnitSyntax;
        var root = tree.DescendantNodesAndSelf()
            .OfType<MethodDeclarationSyntax>().FirstOrDefault();
        return root;
    }
}