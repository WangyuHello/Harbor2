using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Harbor.Core.Tool.LC
{
    public class LCRunner : HarborTool<LCRunnerSettings>
    {
        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "lc_shell" };

        protected override string GetToolName() => "LC";

        public LCRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) 
            : base(fileSystem, environment, processRunner, tools)
        {
        }
    }
}
