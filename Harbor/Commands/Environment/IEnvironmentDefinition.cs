using System.Collections.Generic;

namespace Harbor.Commands.Environment
{
    public interface IEnvironmentDefinition
    {
        public List<(string app, string version)> Apps { get; set; }
        public List<string> Paths { get; set; }
        public List<string> LdLibraryPaths { get; set; }
        public Dictionary<string, string> AdditionalVariable { get; set; }
    }
}
