using System.Collections.Generic;
// ReSharper disable UnusedMember.Global
// ReSharper disable ArrangeTrailingCommaInMultilineLists

namespace Harbor.Core.Environment
{
    public class Cadence617Environment : IEnvironmentDefinition
    {
        public List<(string app, string version)> Apps { get; set; } = new()
        {
            ("virtuoso", "617")
        };

        public List<string> Paths { get; set; } = new()
        {
            "/apps/EDAs/cadence/IC617",
            "/apps/EDAs/cadence/IC617/tools/bin",
            "/apps/EDAs/cadence/IC617/tools/dfII/bin",
            "/apps/EDAs/cadence/SPECTRE19/tools/bin",
        };

        public List<string> LdLibraryPaths { get; set; } = new()
        {
            "/apps/EDAs/cadence/IC617/tools/inca/lib/64bit",
            "/apps/EDAs/cadence/IC617/tools/lib",
            "/apps/EDAs/cadence/IC617/tools/dfII/lib",
        };

        public Dictionary<string, string> AdditionalVariable { get; set; } = new()
        {
            ["cdsdir"] = "/apps/EDAs/cadence/IC617",
            ["cds_root"] = "/apps/EDAs/cadence/IC617",
            ["SPECTREHOME"] = "/apps/EDAs/cadence/SPECTRE19",
        };
    }
}
