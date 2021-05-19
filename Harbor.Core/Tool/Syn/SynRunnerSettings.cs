using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Cake.Common.IO;
using Harbor.Core;
using Harbor.Common.Project;
using Harbor.Core.Tool.DC;
using Newtonsoft.Json.Linq;
using Harbor.Core.Util;
using Path = System.IO.Path;
using Harbor.Core.Tool.Syn.Model;
using Harbor.Core.Tool.Syn.Tcl;

namespace Harbor.Core.Tool.Syn
{
    public class PortSetting
    {
        public string Name { get; set; }
        public string LoadOf { get; set; }
        public double LoadFactor { get; set; } = 5;
    }

    public class SynRunnerSettings : HarborToolSettings
    {
        public DirectoryPath ProjectPath { get; set; }
        public FilePathCollection Verilog { get; set; }
        public FilePathCollection AdditionalTimingDb { get; set; } = new FilePathCollection();
        public string Top => ProjectInfo.Project;
        public string Clock { get; set; } = "vclk";
        public string Reset { get; set; }
        public double ClockPeriod { get; set; } = 10;
        public double? ClockLatency { get; set; }
        public double? ClockUncertainty { get; set; }
        public double? MaxInputDelay { get; set; }
        public double? MinInputDelay { get; set; }
        public double? MaxOutputDelay { get; set; }
        public double? MinOutputDelay { get; set; }

        public int CapacitanceFactor { get; set; } = 5;
        public int LoadFactor { get; set; } = 5;
        public int MaxFanout { get; set; } = 40;
        public double? MaxTransition { get; set; }
        public double MaxArea { get; set; }
        public int CriticalRange { get; set; } = 3;
        public int TimingReportNumber { get; set; } = 50;

        public bool AddPG { get; set; } = true;
        public bool STA { get; set; }
        public bool AllowTriState { get; set; }

        public List<PortSetting> PortSettings { get; set; } = new List<PortSetting>();

        public FilePath BuildScriptFile { get; set; }

        public DCRunnerSettings GetDcRunnerSettings()
        {
            var dcRunnerSettings = new DCRunnerSettings
            {
                WorkingDirectory = WorkingDirectory, 
                CommandFile = BuildScriptFile,
                CommandLogFile = CommandLogFile
            };
            return dcRunnerSettings;
        }

        private Library library;

        internal override void GenerateTclScripts()
        {
            var model = new BuildTclModel();

            // ProjectPath eg. ./Synthesize
            var scriptRootPath = ProjectPath.Combine("build");

            model.ScriptRootPath = scriptRootPath.FullPath;
            model.WorkPath = ProjectPath.Combine("lib").FullPath;
            model.RptPath = ProjectPath.Combine("rpt").FullPath;
            model.NetPath = ProjectPath.Combine("netlist").FullPath;
            model.SvfPath = ProjectPath.Combine("svf").FullPath;
            model.ObjPath = ProjectPath.Combine("obj").FullPath;
            model.MemPath = ProjectPath.Combine("mem").FullPath;
            model.Cores = Environment.ProcessorCount < 16 ? Environment.ProcessorCount : 16;

            IOHelper.CreateDirectory(ProjectPath);
            IOHelper.CreateDirectory(scriptRootPath);
            IOHelper.CreateDirectory(model.WorkPath);
            IOHelper.CreateDirectory(model.RptPath);
            IOHelper.CreateDirectory(model.NetPath);
            IOHelper.CreateDirectory(model.SvfPath);
            IOHelper.CreateDirectory(model.ObjPath);

            library = AllLibrary.GetLibrary(ProjectInfo);

            model.LibPath = library.PrimaryStdCell.timing_db_path;
            model.LibName = library.PrimaryStdCell.timing_db_name_abbr;
            model.LibFullName = library.PrimaryStdCell.timing_db_name;

            if (library.Io is {Count: > 0})
            {
                model.IOTimingDbPaths = library.Io.Select(i => Path.Combine(i.timing_db_path, i.timing_db_name)).ToList();
            }

            model.InvName = library.PrimaryStdCell.primitive_inv;
            model.InvPortName = library.PrimaryStdCell.primitive_inv_port;

            model.TopName = Top;
            model.SourceFullPaths = Verilog?.Select(f => f.FullPath).ToList();

            AdditionalTimingDb = ProjectUtil.GetReferenceDb(AdditionalTimingDb, ProjectInfo);
            model.AdditionalTimingDbPaths = AdditionalTimingDb?.Select(f => f.FullPath).ToList();

            model.ClkName = Clock.ToUpper();
            model.ClkPeriod = ClockPeriod;
            model.ClkLatency = ClockLatency ?? 0;
            model.ClkUncertainty = ClockUncertainty ?? ClockPeriod * 0.01;
            model.MaxInputDelay = MaxInputDelay ?? ClockPeriod * 0.01;
            model.MinInputDelay = MinInputDelay ?? ClockPeriod * -0.01;
            model.MaxOutputDelay = MaxOutputDelay ?? ClockPeriod * 0.01;
            model.MinOutputDelay = MinOutputDelay ?? ClockPeriod * -0.01;

            model.CapFactor = CapacitanceFactor;
            model.LoadFactor = LoadFactor;
            model.MaxFanout = MaxFanout;
            model.MaxTransition = MaxTransition ?? ClockPeriod * 0.5;
            model.MaxArea = MaxArea;
            model.CriticalRange = CriticalRange;
            model.TimingRptNum = TimingReportNumber;

            model.AllowTriState = AllowTriState;
            model.StdCell = library.PrimaryStdCell;

            model.PortSettings = PortSettings;

            if (ClockPeriod < 5)
            {
                model.UseCompileUltra = true;
            }

            if (!string.IsNullOrEmpty(Reset))
            {
                model.RstName = Reset.ToUpper();
            }

            BuildScriptFile = "build.tcl";
            CommandLogFile = "build.log";
            WorkingDirectory = model.ScriptRootPath;

            var buildTcl = new BuildTcl(model);
            buildTcl.WriteToFile(scriptRootPath.CombineWithFilePath(BuildScriptFile).FullPath);
        }
    }
}
