using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Harbor.Core.Tool.V2LVS
{
    public class V2LVSRunnerSettings : HarborToolSettings
    {
        public FilePath VerilogFile { get; set; }
        public FilePath OutputSpiceFile { get; set; }
        public FilePathCollection VerilogLibraryFiles { get; set; }
        public FilePathCollection SpiceLibraryFiles { get; set; }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            args.Append("v", VerilogFile);
            args.Append("l", VerilogLibraryFiles);
            args.Append("s", SpiceLibraryFiles);
            args.Append("o", OutputSpiceFile);
        }
    }

    public class V2LVSRunner : HarborTool<HarborToolSettings>
    {
        public V2LVSRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        protected override string GetToolName() => "V2LVS";

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "v2lvs" };
    }
}
