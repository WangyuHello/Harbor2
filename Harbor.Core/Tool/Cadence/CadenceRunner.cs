using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.Cadence
{ 
    public class CadenceRunnerSettings : HarborToolSettings { }

    public class CadenceRunner : HarborTool<CadenceRunnerSettings>
    {
        public CadenceRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {

        }

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "CreateCadenceProject.ps1" };

        protected override string GetToolName() => "Create Cadence Project";

        protected override void ProcessExitCode(int exitCode)
        {
            switch (exitCode)
            {
                case 1:
                    throw new CakeException($"创建Cadence项目出错");
                default:
                    break;
            }
        }
    }
}
