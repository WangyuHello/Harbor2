using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
// ReSharper disable InconsistentNaming

namespace Harbor.Core.Tool.Calibre
{
    public class CalibreRunnerSettings : HarborToolSettings
    {
        public bool DRC { get; set; }
        public bool LVS { get; set; }
        public bool Hierarchy { get; set; } = true;
        public bool Turbo { get; set; } = true;
        public bool Hyper { get; set; } = true;
        public FilePath RuleFile { get; set; }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            args.Append("drc", DRC);
            args.Append("lvs", LVS);
            args.Append("hier", Hierarchy);
            args.Append("turbo", Turbo);
            args.Append("hyper", Hyper);
            if (RuleFile != null)
            {
                args.Append(RuleFile.FullPath);
            }
        }
    }

    public class CalibreRunner : HarborTool<HarborToolSettings>
    {
        public CalibreRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        protected override string GetToolName() => "Calibre";

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "calibre" };
    }
}
