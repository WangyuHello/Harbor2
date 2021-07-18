using System.Collections.Generic;
// ReSharper disable UnusedMember.Global
// ReSharper disable ArrangeTrailingCommaInMultilineLists

namespace Harbor.Commands.Environment
{
    public class Cadence618Environment : IEnvironmentDefinition
    {
        public List<(string app, string version)> Apps { get; set; } = new()
        {
            ("virtuoso", "618")
        };

        public List<string> Paths { get; set; } = new()
        {
            "/apps/EDAs/cadence/IC618.170",
            "/apps/EDAs/cadence/IC618.170/tools/bin",
            "/apps/EDAs/cadence/IC618.170/tools/dfII/bin",
            "/apps/EDAs/cadence/SPECTRE20.10.126/tools/bin",
        };

        public List<string> LdLibraryPaths { get; set; } = new()
        {
            "/apps/EDAs/cadence/IC618.170/tools/inca/lib/64bit",
            "/apps/EDAs/cadence/IC618.170/tools/lib",
            "/apps/EDAs/cadence/IC618.170/tools/dfII/lib",
        };

        public Dictionary<string, string> AdditionalVariable { get; set; } = new()
        {
            ["cdsdir"] = "/apps/EDAs/cadence/IC618.130",
            ["cds_root"] = "/apps/EDAs/cadence/IC618.130",
            ["SPECTREHOME"] = "/apps/EDAs/cadence/SPECTRE20.10.126",
        };
    }
}
