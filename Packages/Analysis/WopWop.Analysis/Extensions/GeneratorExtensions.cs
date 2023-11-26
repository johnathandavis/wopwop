using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace WopWop.Analysis.Extensions;

public static class GeneratorExtensions
{
    public const string AttributeName = "WopWopMachineManifestAttribute";
    public static void EmitWopWopAttribute(this GeneratorExecutionContext gen, string ns)
    {
        string src = $$"""
                       namespace {{ns}};
                       
                       using WopWop.Analysis.Structure;

                       [System.AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
                       sealed class WopWopMachineManifestAttribute<T> : Attribute where T : IAssemblyMachineManifest
                       {
                           public WopWopMachineManifestAttribute()
                           {
                           }
                       }

                       """;
        gen.AddSource("WopWopMachineManifestAttribute.cs", SourceText.From(src, Encoding.UTF8));


        string usage = $$"""
                         using {{ns}};
                         [assembly:WopWopMachineManifest<MachineManifest>()]
                         """;
        gen.AddSource("AssemblyInfo.WopWopAttribute.cs", SourceText.From(usage, Encoding.UTF8));

    }
}