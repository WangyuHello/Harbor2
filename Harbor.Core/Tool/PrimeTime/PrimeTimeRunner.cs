using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Harbor.Core.Project;
using Harbor.Core.Tool.PrimeTime.Model;
using Harbor.Core.Tool.Syn;
using Harbor.Core.Util;

namespace Harbor.Core.Tool.PrimeTime
{
    public class PrimeTimeRunnerSettings : HarborToolSettings
    {
        //Usage: /apps/EDAs/synopsys/2018/pts/O-2018.06-SP5/bin/pt_shell
        //[-constraints] (Starts Primetime Constraints)

        //Usage: /apps/EDAs/synopsys/2018/pts/O-2018.06-SP5/linux64/syn/bin/pt_shell_exec
        //-root_path path_name(Synopsys root path)
        //[-gui]
        //(Start GUI immediately)
        //[-display display_env_value]
        //(Set DISPLAY environment variable)
        //[-file file_name] (Script file to exec after setup)
        //[-no_init] (Don't load any setup files)
        //[-x command] (Execute a command after setup)
        //[-version] (Show product version and exit immediately)
        //[-multi_scenario] (Initialize in multi scenario mode)
        //[-output_log_file file_name]
        //(Output log file name)

        public string RootPath { get; set; }
        public FilePath CommandFile { get; set; }
        public string Command { get; set; }
        public string Display { get; set; }
        public FilePath OutputLogFile { get; set; }
        public bool Version { get; set; }
        public bool NoInit { get; set; }
        public bool MultiScenario { get; set; }
        public bool Help { get; set; }
        public bool GUI { get; set; }

        public bool APRorSyn { get; set; }
        //public APRRunnerSettings APRSettings { get; set; }
        public SynRunnerSettings SynSettings { get; set; }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            if (!string.IsNullOrWhiteSpace(RootPath))
                args.Append($"-root_path {RootPath}");
            if (!string.IsNullOrWhiteSpace(CommandFile.FullPath))
                args.Append($"-file {CommandFile.FullPath}");
            if (!string.IsNullOrWhiteSpace(Command))
                args.Append($"-x {Command}");
            if (!string.IsNullOrWhiteSpace(Display))
                args.Append($"-display {Display}");
            if (!string.IsNullOrEmpty(OutputLogFile.FullPath))
                args.Append($"-output_log_file {Display}");
            if (Version)
                args.Append("-version");
            if (Help)
                args.Append("-help");
            if (MultiScenario)
                args.Append("-multi_scenario");
            if (NoInit)
                args.Append("-no_init");
            if (GUI)
                args.Append("-gui");
        }

        internal override void GenerateTclScripts()
        {
            if (SynSettings == null) return;

            WorkingDirectory = SynSettings.ProjectPath.Combine("STA");
            var library = AllLibrary.GetLibrary(ProjectInfo);

            CommandFile = WorkingDirectory.CombineWithFilePath("build.tcl");

            var model = new PrimeTimeModel
            {
                APRorSyn = false,
                LibPath = library.PrimaryStdCell.timing_db_path,
                LibName = library.PrimaryStdCell.timing_db_name_abbr,
                LibFullName = library.PrimaryStdCell.timing_db_name,
                TopName = SynSettings.Top,
                SynNetlist = SynSettings.ProjectPath.Combine("netlist").FullPath,
                //APRNetlist = APRSettings.ProjectPath.Combine("netlist").FullPath,
                ScriptRootPath = WorkingDirectory.FullPath,
                ClkName = SynSettings.Clock,
                ClkUncertaintyHold = SynSettings.ClockUncertainty,
                ClkUncertaintySetup = SynSettings.ClockUncertainty,
                MaxTransition = SynSettings.MaxTransition
            };

            var primeTime = new Tcl.PrimeTime(model);
            var tran = primeTime.TransformText();

            File.WriteAllText(CommandFile.FullPath, tran);
        }
    }

    public class PrimeTimeRunner : HarborTool<PrimeTimeRunnerSettings>
    {
        public PrimeTimeRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {

        }

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "pt_shell" };

        protected override string GetToolName() => "PrimeTime";
    }
}
