using System.Collections.Generic;

namespace Harbor.Commands.Environment
{
    public class CadenceEnvironment : IEnvironmentDefinition
    {
        public List<string> Apps { get; set; } = new()
        {
            "virtuoso"
        };

        public List<string> Paths { get; set; } = new()
        {
            "/apps/EDAs/cadence/IC618.170",
            "/apps/EDAs/cadence/IC618.170/tools/bin",
            "/apps/EDAs/cadence/IC618.170/tools/dfII/bin",
            "/apps/EDAs/cadence/SPECTRE19/tools/bin",
        };

        public List<string> LdLibraryPaths { get; set; } = new()
        {
            "/apps/EDAs/cadence/IC618.170/tools/inca/lib/64bit",
            "/apps/EDAs/cadence/IC618.170/tools/lib",
            "/apps/EDAs/cadence/IC618.170/tools/dfII/lib",
        };

        public Dictionary<string, string> AdditionalVariable { get; set; } = new()
        {
            ["CDS_LIC_FILE"] = "5280@dellr900g",
            ["CDS_AUTO_64BIT"] = "ALL",
            ["cdsdir"] = "/apps/EDAs/cadence/IC618.130",
            ["cds_root"] = "/apps/EDAs/cadence/IC618.130",
            ["CDS_Netlisting_Mode"] = "Analog",
            ["SPECTREHOME"] = "/apps/EDAs/cadence/SPECTRE20.10.126",
            ["CDS_LOG_VERSION"] = "sequential",
            ["CDS_USE_XVFB"] = "1",
        };
    }
}
