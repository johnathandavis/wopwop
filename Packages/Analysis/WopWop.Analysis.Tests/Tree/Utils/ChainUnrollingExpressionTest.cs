using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WopWop.Analysis.Tests.Steps;
using WopWop.Analysis.Tree.Chains;

namespace WopWop.Analysis.Tests.Tree.Utils;

public class ChainUnrollingExpressionTest
{
    [Fact]
    public void UnrollsIfExpressionCorrectly()
    {
        string surroundedSource = $$"""
                                    public class Program
                                    {
                                        public async Task<string> RunAsync()
                                        {
                                             int x = new Random().Next(100);
                                             if (x == 4)
                                             {
                                                await Delay(500);
                                             }
                                             else if (x == 5)
                                             {
                                                if (x % 3 == 0)
                                                {
                                                    await.Delay(9000);
                                                }
                                            }
                                            else
                                            {
                                                await Delay(500);
                                            }
                                    }
                                    """;
        
        var syntaxTree = CSharpSyntaxTree.ParseText(surroundedSource);
        var root = (syntaxTree.GetRoot() as CompilationUnitSyntax)!;

        var methDecl = root.DescendantNodes().OfType<IfStatementSyntax>().First();
        var unrolled = ConditionalChainExpansion.UnrollChain(methDecl);
        var ifSyn = unrolled.If;
        Assert.Equal(2, unrolled.ElseIfs.Count);
        Assert.Equal("await Delay(500);", ifSyn.Statement.ToString().Trim());
        Assert.Equal("x == 4", ifSyn.Condition.ToString().Trim());

        var elif1 = unrolled.ElseIfs[0];
        Assert.Equal("x == 4", elif1.ToString().Trim());
        Assert.Equal("""
                     if (x % 3 == 0)
                     {
                         await.Delay(9000);
                     }, 
                     """, unrolled.ElseIfs[0].Statement.ToString().Trim());
    }

}