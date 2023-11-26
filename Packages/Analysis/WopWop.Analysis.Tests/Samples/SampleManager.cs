using System.Reflection;

namespace WopWop.Analysis.Tests.Samples;

public class SampleManager
{
    private static Type MyType = typeof(SampleManager);
    private static Assembly MyAssembly = MyType.Assembly;

    public static string IdentifiesCustomTaskType => GetResource("IdentifiesCustomTaskType.cs");
    public static string TracksVariables => GetResource("TracksVariables.cs");
    public static string DifferentiatesDatumKind => GetResource("DifferentiatesDatumKind.cs");
    public static string StepWithInlineAsync => GetResource("StepWithInlineAsync.cs");
    public static string AsyncInBlock => GetResource("AsyncInBlock.cs");

    private static string GetResource(string name)
    {
        string fullName = typeof(SampleManager).Namespace + "." + name;
        using var s = MyAssembly.GetManifestResourceStream(fullName);
        using var reader = new StreamReader(s);
        return reader.ReadToEnd();
    }
}