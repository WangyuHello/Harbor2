using System.Collections.Generic;
// ReSharper disable UnusedMember.Global
// ReSharper disable ArrangeTrailingCommaInMultilineLists

namespace Harbor.Core.Environment
{
    public class SynopsysEnvironment : IEnvironmentDefinition
    {
        public List<(string app, string version)> Apps { get; set; } = new()
        {
            ("dc_shell", ""),
            ("icc_shell", ""),
            ("lc_shell", ""),
            ("Milkyway", ""),
            ("verdi", ""),
            ("vcs", ""),
        };

        public List<string> Paths { get; set; } = new()
        {
            "/apps/EDAs/synopsys/2020/syn/R-2020.09-SP5/bin",
            "/apps/EDAs/synopsys/2020/vcs/R-2020.12-SP2/bin",
            "/apps/EDAs/synopsys/2020/icc/R-2020.09-SP5/bin",
            "/apps/EDAs/synopsys/2020/mw/R-2020.09-SP5/bin/AMD.64",
            "/apps/EDAs/synopsys/2020/verdi/R-2020.12-SP2/bin",
            "/apps/EDAs/synopsys/2020/lc/R-2020.09-SP5/bin",
        };

        public List<string> LdLibraryPaths { get; set; } = new()
        {
            "/apps/EDAs/synopsys/2020/lc/R-2020.09-SP5/linux64/lc/shlib",
        };

        public Dictionary<string, string> AdditionalVariable { get; set; } = new()
        {
            ["LM_LICENSE_FILE"] = "5280@192.168.69.76:5280@192.168.69.103:2000@192.168.69.2:5280@192.168.69.154:5280@192.168.69.248:5280@192.168.69.247:5280@192.168.69.82",
            ["SNPSLMD_LICENSE_FILE"] = "5280@192.168.69.248:5280@192.168.69.246",
            ["SYNOPSYS"] = "/apps/EDAs/synopsys/2020/syn/R-2020.09-SP5",
            ["VCS_HOME"] = "/apps/EDAs/synopsys/2020/vcs/R-2020.12-SP2",
            ["ICC_HOME"] = "/apps/EDAs/synopsys/2020/icc/R-2020.09-SP5",
            ["MILKYWAY_HOME"] = "/apps/EDAs/synopsys/2020/mw/R-2020.09-SP5",
            ["VERDI_HOME"] = "/apps/EDAs/synopsys/2020/verdi/R-2020.12-SP2",
            ["SYNOPSYS_LC_ROOT"] = "/apps/EDAs/synopsys/2020/lc/R-2020.09-SP5",
        };
    }
}
