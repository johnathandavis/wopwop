using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WopWop.Analysis.Structure;
using WopWop.Analysis.Tree;
using Xunit.Abstractions;

namespace WopWop.Analysis.Tests.Steps;

public class StepFinderTest : CodeTester<MachineAnnotator>
{
    public StepFinderTest(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }
    
    [Fact]
    public void FindsAsyncSteps()
    {
        var steps = CreateStepsFromMethodBody(
            """
                     int x = 5;
                     await Task.Delay(2000);                                
            """);
        Assert.Equal(3, steps.Count);
        Assert.Equal("""
                     int x = 5;
                     await Task.Delay(1000)
                     """, steps[0].Code);
        Assert.Equal("Task.Delay(1000)", steps[0].AwaitedExpression);
        Assert.Equal("""
                    await Task.Delay(2000)
                    """, steps[0].Code);
        Assert.Equal("Task.Delay(2000)", steps[1].AwaitedExpression);
    }
    

    [Fact]
    public void StepCodeIndentationIsRelativeToBlocks()
    {
        var steps = CreateStepsFromMethodBody(
            """
            if (parameter % 2 == 0)
            {
                await Task.Delay(1000);
            }
            else
            {
                Console.WriteLine("No delay for you.");
            }
        
            return await DelayWithRandomResultAsync();
            """);
        Assert.Equal(2, steps.Count);
        Assert.Equal(
            """
                if (parameter % 2 == 0)
                {
                    await Task.Delay(1000)
            """, steps[0].Code);
    }

    private List<MachineStep> CreateStepsFromMethodBody(string str)
    {
        string surroundedSource = $$"""
                                    public class Program
                                    {
                                        public async Task<string> RunAsync()
                                        {
                                             {{str}}
                                        }
                                    }
                                    """;
        var syntaxTree = CSharpSyntaxTree.ParseText(surroundedSource);
        var root = (syntaxTree.GetRoot() as CompilationUnitSyntax)!;
        var method = root.DescendantNodes().OfType<MethodDeclarationSyntax>().First();
        return new List<MachineStep>();
    }
    
    
    private void WalkSyntaxFromMethodBody(string str)
    {
        string surroundedSource = $$"""
                                    public class Program
                                    {
                                        public async Task<string> RunAsync()
                                        {
                                             {{str}}
                                        }
                                    }
                                    """;
        
        var syntaxTree = CSharpSyntaxTree.ParseText(surroundedSource);
        var root = (syntaxTree.GetRoot() as CompilationUnitSyntax)!;

        new SyntaxWalker().DefaultVisit(root);
    }
    
    public class SyntaxWalker : CSharpSyntaxWalker
    {
        public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            
            base.VisitConditionalExpression(node);
        }

        public override void VisitAwaitExpression(AwaitExpressionSyntax node)
        {
            base.VisitAwaitExpression(node);
        }
    }
}