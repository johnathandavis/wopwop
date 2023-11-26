namespace WopWop.Analysis.Structure;

public class MachineIdentifier
{
    public MachineIdentifier(
        string typeName,
        string methodName,
        string methodSignature)
    {
        this.TypeName = typeName;
        this.MethodName = methodName;
        this.MethodSignature = methodSignature;
    }
    
    public string TypeName { get; private set; }
    public string MethodName { get; private set; }
    public string MethodSignature { get; private set; }
}