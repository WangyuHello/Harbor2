using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Harbor.Core.Tool.LC.Model;
using Harbor.Core.Tool.LC.Tcl;

namespace Harbor.Core.Tool.LC
{
    public class LCRunnerSettings : HarborToolSettings
    {
        public string ProjectName { get; set; }
        public string ProjectLibFilePath { get; set; }
        public FilePath CommandFile { get; set; }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            if (!string.IsNullOrWhiteSpace(CommandFile.FullPath))
                args.Append($"-f {CommandFile.FullPath}");
        }

        internal override void GenerateRunScripts()
        {
            BuildTclModel model = new BuildTclModel
            {
                ProjectLibFilePath = ProjectLibFilePath,
                ProjectName = ProjectName,
            };

            CommandFile = "build.tcl";

            BuildTcl buildTcl = new BuildTcl(model);
            var tran = buildTcl.TransformText();

            File.WriteAllText(WorkingDirectory.CombineWithFilePath(CommandFile).FullPath, tran);
        }
    }
}
