using WopWop.Analysis.Tests.Samples;
using Xunit.Abstractions;

namespace WopWop.Analysis.Tests;

public class CandidateAnalyzerTest : CodeTester<MachineAnnotator>
{
    public CandidateAnalyzerTest(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void IdentifiesAsyncMethodsUsingCustomTask()
    {
        var compilation = RunGenerator(SampleManager.IdentifiesCustomTaskType, out _, out _);
        var candidates = compilation.ToCandidates();
        Assert.Single(candidates);
        var c = candidates[0];
        Assert.Equal("Program", c.ClassSyntax.Identifier.Text);
        Assert.Equal("ProcessAsync", c.MethodSyntax.Identifier.Text);
    }
}