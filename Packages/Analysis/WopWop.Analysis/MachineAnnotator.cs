using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using WopWop.Analysis.Extensions;
using WopWop.Analysis.Structure;

namespace WopWop.Analysis;

[Generator]
public class MachineAnnotator : ISourceGenerator
{
    
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new CandidateDetector());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var ns = context.Compilation.Assembly.Name;

        if (ns.StartsWith("WopWop.Analysis"))
        {
            return;
        }
        
        CandidateDetector detector = (CandidateDetector)context.SyntaxReceiver!;
        var candidates = EnrichedMachineCandidates(
            context.Compilation,
            detector.AsyncMethods);

        var machines = new List<MachineStructure>();
        foreach (var c in candidates)
        {
            if (CandidateAnalyzer.TryGetMachineStructure(c, out var mc))
            {
                machines.Add(mc);
            }
        }


        var machineSb = new StringBuilder();
        foreach (var machine in machines)
        {
            machineSb.AppendLine($"""ls.Add({machine.ToManifestBuilderString()});""");
        }

        string manifestCode = $$"""
using System.Collections.Generic;

using WopWop.Analysis.Structure;
using WopWop.Analysis.Structure.Builder;

namespace {{ns}}
{
    internal class MachineManifest : IAssemblyMachineManifest
    {
        internal static IReadOnlyList<MachineStructure> InitializedMachines { get; }

        static MachineManifest()
        {
            var ls = new List<MachineStructure>();
            {{machineSb}}
            InitializedMachines = ls;
        }

        public IReadOnlyList<MachineStructure> Machines => MachineManifest.InitializedMachines;
    }
}
""";
        context.AddSource("MachineManifest.g.cs", SourceText.From(manifestCode, Encoding.UTF8));
        context.EmitWopWopAttribute(ns);
    }

    internal static List<EnrichedMachineCandidate> EnrichedMachineCandidates(Compilation compilation,
        Dictionary<ClassDeclarationSyntax, List<MachineCandidate>> candidateDict)
    {
        var enriched = new List<EnrichedMachineCandidate>();
        foreach (var kvp in candidateDict)
        {
            var classSyntax = kvp.Key;
            var candidates = kvp.Value;
            var model = compilation.GetSemanticModel(classSyntax.SyntaxTree);
            foreach (var candidate in candidates)
            {
                var flow = model.AnalyzeDataFlow(candidate.MethodSyntax.Body);
                enriched.Add(new EnrichedMachineCandidate(candidate, flow, model));
            }
        }

        return enriched;
    }
}