using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Harbor.Core.Tool.Milkyway
{
    public class MilkywayRunner : HarborTool<MilkywayRunnerSettings>
    {
        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "Milkyway" };

        protected override string GetToolName() => "Milkyway";

        public MilkywayRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) 
            : base(fileSystem, environment, processRunner, tools)
        {
        }
    }
}
