using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Commands.Environment
{
    public class SynopsysEnvironment : IEnvironmentDefinition
    {
        public List<string> Apps { get; set; } = new()
        {
            "dc_shell",
            "icc_shell"
        };

        public List<string> Paths { get; set; }
        public List<string> LdLibraryPaths { get; set; }
        public Dictionary<string, string> AdditionalVariable { get; set; }
    }
}
