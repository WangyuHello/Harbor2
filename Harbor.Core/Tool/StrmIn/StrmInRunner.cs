using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.StrmIn
{
    public enum StrmInCase
    {
        upper,
        lower,
        preserve
    }


    public class StrmInRunnerSettings : HarborToolSettings
    {
        public string Library { get; set; }
        public FilePath StrmFile { get; set; }
        public FilePath LogFile { get; set; }
        public string View { get; set; }
        public StrmInCase Case { get; set; } = StrmInCase.preserve;
        public bool ReplaceBusBitChar { get; set; } //Replace "[]" With "<>"
        public FilePath LayerMapFile { get; set; }
        public FilePath RefLibList { get; set; }
        public string AttachTechFileOfLib { get; set; }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            args.Append("library", Library);
            args.Append("strmFile", StrmFile);
            args.Append("logFile", LogFile);
            args.Append("view", View);
            args.Append("case", Case);
            args.Append("replaceBusBitChar", ReplaceBusBitChar);
            args.Append("layerMap", LayerMapFile);
            args.Append("refLibList", RefLibList);
            args.Append("attachTechFileOfLib", AttachTechFileOfLib);
        }
    }

    public class StrmInRunner : HarborTool<StrmInRunnerSettings>
    {
        public StrmInRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {

        }

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "strmin" };

        protected override string GetToolName() => "StrmIn";

    }
}
