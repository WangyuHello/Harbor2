using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.AddPG
{
    public class AddPGRunnerSettings : HarborToolSettings
    {
        public FilePath File { get; set; }
        public string Library { get; set; }
        public string StdCell { get; set; }
        public FilePath ProjectJson { get; set; }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            if (!string.IsNullOrEmpty(Library))
                args.Append($"-L {Library}");
            if (!string.IsNullOrEmpty(StdCell))
                args.Append($"-S {StdCell}");
            if (ProjectJson != null)
                args.Append($"-P {ProjectJson.FullPath}");
            args.Append(File.FullPath);
        }
    }

    public class AddPGRunner : HarborTool<AddPGRunnerSettings>
    {
        public AddPGRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {

        }

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "add-pg.py" };

        protected override string GetToolName() => "AddPG";

        protected override void ProcessExitCode(int exitCode)
        {
            switch (exitCode)
            {
                case 1:
                    throw new CakeException($"Parse 文件出错");
                default:
                    break;
            }
        }
    }
}
