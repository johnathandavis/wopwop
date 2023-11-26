using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit.Abstractions;
using GCExtensions = WopWop.Analysis.Extensions.GeneratorExtensions;
namespace WopWop.Analysis.Tests;

extern alias WopWopAnalysis;

public class MachineAnnotatorTest : CodeTester<MachineAnnotator>
{
    public MachineAnnotatorTest(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public void CreatesManifestAttribute()
    {
        var userSource = @"";
        var compilation = RunGenerator(userSource, out _, out var files);
        var attributeSt = compilation.SyntaxTrees
            .First(st => st.FilePath.Contains(GeneratorExtensions.AttributeName))
            .GetRoot();
        var declarationNode = attributeSt.DescendantNodes().OfType<ClassDeclarationSyntax>().First();
        Assert.Equal(GeneratorExtensions.AttributeName, declarationNode.Identifier.ValueText);
        Assert.Equal(1, declarationNode.Arity);
        Assert.Equal("T", declarationNode.TypeParameterList.Parameters.First().Identifier.ValueText);
        Assert.True(declarationNode.Modifiers.Any((a) => a.IsKind(SyntaxKind.SealedKeyword)));
        Assert.Equal(1, declarationNode.BaseList.Types.Count);
        Assert.Equal("Attribute", declarationNode.BaseList.Types[0].Type.ToString());
        Assert.Equal(1, declarationNode.ConstraintClauses.Count);
        Assert.Equal("T", declarationNode.ConstraintClauses[0].Name.Identifier.ValueText);
        Assert.Equal(1, declarationNode.ConstraintClauses[0].Constraints.Count);
        Assert.True(declarationNode.ConstraintClauses[0].Constraints[0].IsKind(SyntaxKind.TypeConstraint));
        var tsConstraint = declarationNode.ConstraintClauses[0].Constraints[0] as TypeConstraintSyntax;
        Assert.Equal("IAssemblyMachineManifest", tsConstraint.Type.ToString());
        Assert.Equal(3, files.Length);
    }
}