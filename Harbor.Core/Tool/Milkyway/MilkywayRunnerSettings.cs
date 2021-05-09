using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Harbor.Common.Project;
using Harbor.Core.Tool.Milkyway.Model;
using Harbor.Core.Tool.Milkyway.Tcl;
using Harbor.Core.Util;
using Newtonsoft.Json.Linq;

namespace Harbor.Core.Tool.Milkyway
{
    public class MilkywayRunnerSettings : HarborToolSettings
    {
        public string ProjectName { get; set; }
        public string ProjectLefFilePath { get; set; }
        public bool NullDisplay { get; set; }
        public FilePath CommandFile { get; set; }

        public ProcessArgumentBuilder ToArgs()
        {
            var args = new ProcessArgumentBuilder();
            if (!string.IsNullOrWhiteSpace(CommandFile.FullPath))
                args.Append($"-load {CommandFile.FullPath}");
            if (NullDisplay)
            {
                args.Append("-nullDisplay");
            }
            return args;
        }

        internal override void GenerateTclScripts()
        {
            var library = AllLibrary.GetLibrary(ProjectInfo);
            BuildTclModel model = new BuildTclModel
            {
                ProjectLefFilePath = ProjectLefFilePath,
                ProjectName = ProjectName,
                TechFilePath = library.PrimaryStdCell.techfile_full_name,
                TechLefFilePath = library.PrimaryStdCell.techlef_file_full_name
            };

            CommandFile = "build.cmd";
            CommandLogFile = "build.log";

            BuildTcl buildTcl = new BuildTcl(model);
            var tran = buildTcl.TransformText();

            File.WriteAllText(WorkingDirectory.CombineWithFilePath(CommandFile).FullPath, tran);
        }
    }
}
