using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.Virtuoso
{
    public class VirtuosoRunnerSettings : HarborToolSettings
    {
        public bool NoGraph { get; set; }
        public FilePath RestoreFile { get; set; }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            args.Append("nograph", NoGraph);
            args.Append("restore", RestoreFile);
            args.Append("log", CommandLogFile);
        }
    }

    public class VirtuosoRunner : HarborTool<VirtuosoRunnerSettings>
    {
        public VirtuosoRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {

        }

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "virtuoso" };

        protected override string GetToolName() => "Virtuoso";
    }
}
