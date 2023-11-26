using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace WopWop.Core.Info;

public class StateMachineRuntimeInfo
{
    private readonly IAsyncStateMachine sm;
    private readonly Type _type;
    private readonly FieldInfo[] _fields;
    private FieldInfo _stateField;
    private FieldInfo _thisField;
    private MethodInfo _method;

    public StateMachineRuntimeInfo(IAsyncStateMachine sm)
    {
        this.sm = sm;
        this._type = sm.GetType();
        this._fields = this._type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        this._stateField = _fields.First(f => f.FieldType == typeof(int) && f.Name.Contains("__state"));
        ///this._thisField = _fields.First(f => f.Name.Contains("__this"));
        string methodName = Regex.Match(this._type.Name, "<(?<methodName>[^>]+)>[a-zA-Z0-9_`]+").Groups["methodName"].Value;
        this._method = this._type.DeclaringType!.GetMethod(methodName)!;
    }

    public int State => (int)_stateField.GetValue(sm)!;
    public Type SrcClass => _type.DeclaringType;
    public MethodInfo SrcMethod => _method;
    public object This => _thisField.GetValue(sm);
}