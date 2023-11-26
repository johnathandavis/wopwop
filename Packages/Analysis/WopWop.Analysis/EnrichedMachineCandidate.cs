using Microsoft.CodeAnalysis;

namespace WopWop.Analysis;

public class EnrichedMachineCandidate : MachineCandidate
{
    public EnrichedMachineCandidate(MachineCandidate c, DataFlowAnalysis a, SemanticModel m) : base(c.ClassSyntax, c.MethodSyntax)
    {
        this.DataFlowAnalysis = a;
        this.Model = m;
    }
    
    public DataFlowAnalysis DataFlowAnalysis { get; private set; }
    public SemanticModel Model { get; private set; }
}