using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.ConvertAMS
{
    public class ConvertAMSRunnerSettings : HarborToolSettings
    {
        public string Top { get; set; }
        public FilePath SourceFile { get; set; }
        public FilePath OutputFile { get; set; }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            if (!string.IsNullOrEmpty(Top))
                args.Append($"-N {Top}");
            args.Append(SourceFile.FullPath);
            args.Append(OutputFile.FullPath);
        }
    }

    public class ConvertAMSRunner : HarborTool<ConvertAMSRunnerSettings>
    {
        public ConvertAMSRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {

        }

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "convert-rtl-to-ams.py" };

        protected override string GetToolName() => "convert-rtl-to-ams";

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
