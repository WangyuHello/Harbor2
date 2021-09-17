using System.Collections.Generic;
// ReSharper disable UnusedMember.Global
// ReSharper disable ArrangeTrailingCommaInMultilineLists

namespace Harbor.Core.Environment
{
    public class CadenceEnvironment : IEnvironmentDefinition
    {
        public List<(string app, string version)> Apps { get; set; } = new()
        {
            ("virtuoso", "")
        };

        public List<string> Paths { get; set; } = new()
        {
        };

        public List<string> LdLibraryPaths { get; set; } = new()
        {

        };

        public Dictionary<string, string> AdditionalVariable { get; set; } = new()
        {
            ["LM_LICENSE_FILE"] = "5280@192.168.69.76:5280@192.168.69.103:2000@192.168.69.2:5280@192.168.69.154:5280@192.168.69.248:5280@192.168.69.247:5280@192.168.69.82",
            ["CDS_LIC_FILE"] = "5280@192.168.69.247",
            ["CDS_AUTO_64BIT"] = "ALL",
            ["CDS_Netlisting_Mode"] = "Analog",
            ["CDS_LOG_VERSION"] = "sequential",
            ["CDS_USE_XVFB"] = "1",
        };
    }
}
