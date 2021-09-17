using System.Collections.Generic;
// ReSharper disable UnusedMember.Global
// ReSharper disable ArrangeTrailingCommaInMultilineLists

namespace Harbor.Core.Environment
{
    public class CadenceAmsEnvironment : IEnvironmentDefinition
    {
        public List<(string app, string version)> Apps { get; set; } = new()
        {
            ("virtuoso", ""),
            ("ncvlog", ""),
        };

        public List<string> Paths { get; set; } = new()
        {
            "/apps/EDAs/cadence/INCISIV15.2/tools/bin",
            "/apps/EDAs/cadence/INCISIV15.2/tools/dfII/bin",
        };

        public List<string> LdLibraryPaths { get; set; } = new()
        {
            "/apps/EDAs/cadence/INCISIV15.2/tools/lib",
        };

        public Dictionary<string, string> AdditionalVariable { get; set; } = new()
        {
            ["LDV"] = "/apps/EDAs/cadence/INCISIV15.2",
        };
    }
}
