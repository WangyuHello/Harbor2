using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.Sed
{
    public class SedRunnerSettings : HarborToolSettings
    {
        public FilePath File { get; set; }
        public string Command { get; set; }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            if (!string.IsNullOrEmpty(Command))
                args.Append($"-i {Command}");
            args.Append(File.FullPath);
        }
    }

    public class SedRunner : HarborTool<SedRunnerSettings>
    {
        public SedRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {

        }

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "sed" };

        protected override string GetToolName() => "Sed";
    }
}
