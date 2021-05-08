using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Harbor.Core.Tool.Ihdl
{
    public class IhdlRunnerSettings : HarborToolSettings
    {
        public FilePath Param { get; set; }
        public string DestIRLib { get; set; }
        public FilePath Verilog { get; set; }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            if (Param != null)
                args.Append($"-param {Param.FullPath}");
            if (!string.IsNullOrEmpty(DestIRLib))
                args.Append($"-destIRLib {DestIRLib}");
            if (Verilog != null)
                args.Append(Verilog.FullPath);
        }
    }

    public class IhdlRunner : HarborTool<HarborToolSettings>
    {
        public IhdlRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        protected override string GetToolName() => "ihdl";

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "ihdl" };
    }
}
