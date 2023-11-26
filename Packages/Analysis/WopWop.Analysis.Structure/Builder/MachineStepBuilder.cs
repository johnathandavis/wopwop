namespace WopWop.Analysis.Structure.Builder;

public class MachineStepBuilder
{
    private string code;
    private string awaitedExpression;

    public MachineStepBuilder WithCode(string code)
    {
        this.code = code;
        return this;
    }

    public MachineStepBuilder WithAwaitedExpression(string awaitedExpression)
    {
        this.awaitedExpression = awaitedExpression;
        return this;
    }

    public MachineStep Build()
    {
        return new MachineStep(this.code, this.awaitedExpression);
    }
}