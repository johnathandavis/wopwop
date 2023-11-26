
using WopWop.Analysis.Structure.Builder;

namespace WopWop.Analysis.Extensions;

using System.Text;

using WopWop.Analysis.Structure;

public static class ManifestExtensions
{
    public static string ToManifestBuilderString(this WopWop.Analysis.Structure.MachineIdentifier machine)
    {
        var stepsSb = new StringBuilder();
        foreach (var s in machine)
        {
        }
        return $"""""
        new {nameof(MachineStructureBuilder)}()
            .{nameof(MachineStructureBuilder.WithId)}(new {nameof(MachineIdentifier)}(
                """{machine.Id.TypeName}""",
                """{machine.Id.MethodName}""",
                """{machine.Id.MethodSignat}""",
        """"))
            {stepsSb}
            .Build()
        """";
    }
}