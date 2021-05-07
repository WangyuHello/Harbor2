using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.GetPorts
{
    public class GetPortsRunnerSettings : HarborToolSettings
    {
        public FilePath File { get; set; }
        public string Top { get; set; }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            if (!string.IsNullOrEmpty(Top))
                args.Append($"-T {Top}");
            args.Append(File.FullPath);
        }
    }

    public class GetPortsRunner : HarborTool<GetPortsRunnerSettings>
    {
        public GetPortsRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {

        }

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "get-ports.py" };

        protected override string GetToolName() => "Get Ports";

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
