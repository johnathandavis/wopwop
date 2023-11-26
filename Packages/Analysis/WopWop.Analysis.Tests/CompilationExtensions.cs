using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WopWop.Analysis.Extensions;
using WopWop.Core.Manifest;

namespace WopWop.Analysis.Tests;

public static class CompilationExtensions
{
    public static List<EnrichedMachineCandidate> ToCandidates(this Compilation c)
    {
        var candidates = new List<EnrichedMachineCandidate>();
        foreach (var st in c.SyntaxTrees)
        {
            var root = st.GetRoot();
            var cs = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .ToDictionary(c => c, c => c.FindCandidates());
            var enriched = MachineAnnotator.EnrichedMachineCandidates(c, cs);
            candidates.AddRange(enriched);
        }

        return candidates;
    }
    public static IMachineManifestLookup ToLookup(this Compilation c)
    {
        using var peStream = new MemoryStream();
        var result = c.Emit(peStream);
        if(!result.Success)
        {
            foreach(var diagnostic in result.Diagnostics)
            {
                Console.WriteLine(diagnostic.ToString());
            }

            throw new ArgumentException("Failed to emit");
        }
        peStream.Seek(0, SeekOrigin.Begin);

        var assemblyBytes = peStream.ToArray();
        
        var assemblyLoadContext = new SimpleUnloadableAssemblyLoadContext();
        var assembly = assemblyLoadContext.LoadFromStream(peStream);
        var lookup = Extractor.GetLookupFromAssembly(assembly);
        assemblyLoadContext.Unload();
        return lookup;
    }
    
    internal class SimpleUnloadableAssemblyLoadContext : AssemblyLoadContext
    {
        public SimpleUnloadableAssemblyLoadContext()
            : base(true)
        {
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}