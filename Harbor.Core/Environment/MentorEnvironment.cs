using System.Collections.Generic;
// ReSharper disable UnusedMember.Global
// ReSharper disable ArrangeTrailingCommaInMultilineLists

namespace Harbor.Core.Environment
{
    public class MentorEnvironment : IEnvironmentDefinition
    {
        public List<(string app, string version)> Apps { get; set; } = new()
        {
            ("virtuoso", ""),
            ("calibre", ""),
            ("calibredrv", ""),
        };

        public List<string> Paths { get; set; } = new()
        {
            "/apps/EDAs/mentor/calibre/aoi_cal_2020.4_34.17/bin",
            "/apps/EDAs/mentor/calibre/aoi_nxdat_2020.4_34.17/bin",
        };

        public List<string> LdLibraryPaths { get; set; } = new()
        {

        };

        public Dictionary<string, string> AdditionalVariable { get; set; } = new()
        {
            ["LM_LICENSE_FILE"] = "5280@192.168.69.76:5280@192.168.69.103:2000@192.168.69.2:5280@192.168.69.154:5280@192.168.69.248:5280@192.168.69.247:5280@192.168.69.82",
            ["MGLS_LICENSE_FILE"] = "5280@192.168.69.76:5280@192.168.69.246",
            ["MGC_HOME"] = "/apps/EDAs/mentor/calibre/aoi_cal_2020.4_34.17",
            ["NXDAT_MGC_HOME"] = "/apps/EDAs/mentor/calibre/aoi_nxdat_2020.4_34.17",
            ["CALIBRE_HOME"] = "/apps/EDAs/mentor/calibre/aoi_cal_2020.4_34.17",
        };
    }
}
