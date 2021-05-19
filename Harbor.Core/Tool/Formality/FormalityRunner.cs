using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Harbor.Core.Tool.APR.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Harbor.Common.Project;
using Harbor.Core.Tool.APR;
using Harbor.Core.Util;
using Path = System.IO.Path;

namespace Harbor.Core.Tool.Formality
{
    public class FormalityRunnerSettings : HarborToolSettings
    {
        //Usage: /apps/EDAs/synopsys/2018/fm/O-2018.06-SP5/linux64/fm/bin/fm_shell_exec
        //        [-root_path path_name] (Synopsys root path)
        //[-work_path path_name] (Formality work directory path)
        //[-32bit] (Use 32 bit executable)
        //[-64bit] (Use 64 bit executable -- default)
        //[-name_suffix file_name_suffix]
        //(Suffix appended to file names created by Formality)
        //[-checkout feature] (Feature requested checked out by Formality)
        //[-no_init] (Don't load any setup files)
        //[-version] (Show product version and exit immediately)
        //[-build] (Show product build and exit immediately)
        //[-session session_file_name]
        //(Session file to restore after setup)
        //[-x command_string] (Command string to exec after setup)
        //[-file file_name] (Script file to exec after setup)
        //[-overwrite] (Replace existing log files and FM_WORK directory)
        //[-gui] (Start the GUI immediately)

        public string WorkPath { get; set; }
        public FilePath CommandFile { get; set; }
        public string Command { get; set; }
        public string Checkout { get; set; }
        public bool Version { get; set; }
        public bool Use64Bit { get; set; }
        public bool Use32Bit { get; set; }
        public bool Help { get; set; }
        public bool GUI { get; set; }

        public APRRunnerSettings APRSettings { get; set; }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            if (!string.IsNullOrWhiteSpace(WorkPath))
                args.Append($"-work_path {WorkPath}");
            if (!string.IsNullOrWhiteSpace(CommandFile.FullPath))
                args.Append($"-file {CommandFile.FullPath}");
            if (!string.IsNullOrWhiteSpace(Command))
                args.Append($"-x {Command}");
            if (!string.IsNullOrWhiteSpace(Checkout))
                args.Append($"-checkout {Checkout}");
            if (Version)
                args.Append("-version");
            if (Use64Bit)
                args.Append("-64bit");
            if (Use32Bit)
                args.Append("-32bit");
            if (Help)
                args.Append("-help");
            if (GUI)
                args.Append("-gui");
        }

        internal override void GenerateTclScripts()
        {
            WorkingDirectory = APRSettings.ProjectPath.Combine("formality");
            var library = AllLibrary.GetLibrary(ProjectInfo);

            IOHelper.CreateDirectory(WorkingDirectory);

            CommandFile = WorkingDirectory.CombineWithFilePath("build.tcl");

            var model = new FormalityModel
            {
                LibPath = library.PrimaryStdCell.timing_db_path,
                LibName = library.PrimaryStdCell.timing_db_name_abbr,
                LibFullName = library.PrimaryStdCell.timing_db_name,
                TopName = APRSettings.Top,
                SynNetlist = APRSettings.SynProjectPath.Combine("netlist").FullPath,
                Netlist = APRSettings.ProjectPath.Combine("netlist").FullPath,
                ScriptRootPath = WorkingDirectory.FullPath
            };

            if (library.Io is {Count: > 0})
            {
                model.IOTimingDbPaths = library.Io.Select(i => Path.Combine(i.timing_db_path, i.timing_db_name)).ToList();
            }

            model.AdditionalTimingDbPaths = APRSettings.AdditionalTimingDb.Select(f => f.FullPath).ToList();

            var formality = new APR.Tcl.Formality(model);
            formality.WriteToFile(CommandFile.FullPath);
        }

    }

    public class FormalityRunner : HarborTool<FormalityRunnerSettings>
    {
        public FormalityRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {

        }

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "fm_shell" };

        protected override string GetToolName() => "Formality";

        protected override void ProcessExitCode(int exitCode)
        {
            switch (exitCode)
            {
                case 1: 
                    throw new CakeException("形式验证失败");
            }
        }
    }
}
