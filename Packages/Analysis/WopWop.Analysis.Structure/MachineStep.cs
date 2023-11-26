namespace WopWop.Analysis.Structure;

public class MachineStep
{
    public MachineStep(string code, string awaitedExpression)
    {
        this.Code = code;
        this.AwaitedExpression = awaitedExpression;
    }
    
    public string Code { get; private set; }
    public string AwaitedExpression { get; private set; }
}