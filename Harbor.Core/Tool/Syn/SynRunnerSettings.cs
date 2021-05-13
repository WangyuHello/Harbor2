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

            if (library.Io != null && library.Io.Count > 0)
            {
                model.IOTimingDbPaths = library.Io.Select(i => Path.Combine(i.timing_db_path, i.timing_db_name)).ToList();
            }

            model.InvName = library.PrimaryStdCell.primitive_inv;
            model.InvPortName = library.PrimaryStdCell.primitive_inv_port;

            model.TopName = Top;
            model.SourceFullPaths = Verilog?.Select(f => f.FullPath).ToList();

            AdditionalTimingDb = GetReferenceDb(AdditionalTimingDb);
            model.AdditionalTimingDbPaths = AdditionalTimingDb?.Select(f => f.FullPath).ToList();

            model.ClkName = Clock.ToUpper();
            model.ClkPeriod = ClockPeriod;
            model.ClkLatency = ClockLatency ?? ClockPeriod * 0.01;
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

        internal FilePathCollection GetReferenceDb(FilePathCollection additionalDb)
        {
            if (ProjectInfo.Reference == null)
            {
                return additionalDb;
            }
            var refs = ProjectInfo.Reference;
            foreach (var ref_ in refs)
            {
                var name = ref_.Name;
                var path = ref_.Path;

                var refProjectInfo = ProjectInfo.ReadFromDirectory(path);
                var refProjectType = refProjectInfo.Type;
                switch (refProjectType)
                {
                    case ProjectType.Analog:
                        break;
                    case ProjectType.Memory: //当前只支持Memory
                        var refLibertyPath = Path.Combine(path, "liberty");
                        var dbs = Directory.GetFiles(refLibertyPath, "*.db", SearchOption.TopDirectoryOnly).Select(p => new FileInfo(p));
                        var ttDb = dbs.FirstOrDefault(db => db.Name.Contains("tt"));
                        if (ttDb != null)
                        {
                            additionalDb.Add(ttDb.FullName);
                        }
                        break;
                    case ProjectType.Digital:
                        break;
                    case ProjectType.Ip:
                        break;
                    default:
                        break;
                }

            }

            return additionalDb;
        }
    }



    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class SynRunnerSettingsBuilder
    {
        public SynRunnerSettings Settings { get; } = new SynRunnerSettings();

        private readonly ICakeContext context;

        public SynRunnerSettingsBuilder(ICakeContext context) 
        {
            this.context = context;
            Settings.Context = context;
        }

        public SynRunnerSettingsBuilder ProjectInfo(ProjectInfo _)
        {
            return this;
        }

        public SynRunnerSettingsBuilder AddPG()
        {
            Settings.AddPG = true;
            return this;
        }

        public SynRunnerSettingsBuilder AllowTriState()
        {
            Settings.AllowTriState = true;
            return this;
        }

        public SynRunnerSettingsBuilder STA()
        {
            Settings.STA = true;
            return this;
        }

        public SynRunnerSettingsBuilder Verilog(FilePathCollection verilog)
        {
            if (Settings.Verilog == null)
            {
                Settings.Verilog = new FilePathCollection();
            }
            Settings.Verilog.Add(verilog);
            return this;
        }

        public SynRunnerSettingsBuilder Verilog(string pattern)
        {
            if (Settings.Verilog == null)
            {
                Settings.Verilog = new FilePathCollection();
            }
            var vs = context.GetFiles(pattern);
            Settings.Verilog.Add(vs);
            return this;
        }

        public SynRunnerSettingsBuilder AddTimingDB(FilePathCollection db)
        {
            if (Settings.AdditionalTimingDb == null)
            {
                Settings.AdditionalTimingDb = new FilePathCollection();
            }
            Settings.AdditionalTimingDb.Add(db);
            return this;
        }

        public SynRunnerSettingsBuilder AddTimingDB(string pattern)
        {
            if (Settings.AdditionalTimingDb == null)
            {
                Settings.AdditionalTimingDb = new FilePathCollection();
            }
            var dbs = context.GetFiles(pattern);
            Settings.AdditionalTimingDb.Add(dbs);
            return this;
        }

        public SynRunnerSettingsBuilder Clock(string clock)
        {
            if (!string.IsNullOrEmpty(clock))
            {
                Settings.Clock = clock;
            }
            return this;
        }

        public SynRunnerSettingsBuilder Reset(string reset)
        {
            Settings.Reset = reset;
            return this;
        }

        public SynRunnerSettingsBuilder ClockPeriod(double clockPeriod)
        {
            Settings.ClockPeriod = clockPeriod;
            return this;
        }

        public SynRunnerSettingsBuilder ClockLatency(double clockLatency)
        {
            Settings.ClockLatency = clockLatency;
            return this;
        }

        public SynRunnerSettingsBuilder ClockUncertainty(double clockUncertainty)
        {
            Settings.ClockUncertainty = clockUncertainty;
            return this;
        }

        public SynRunnerSettingsBuilder MaxInputDelay(double maxInputDelay)
        {
            Settings.MaxInputDelay = maxInputDelay;
            return this;
        }

        public SynRunnerSettingsBuilder MinInputDelay(double minInputDelay)
        {
            Settings.MinInputDelay = minInputDelay;
            return this;
        }

        public SynRunnerSettingsBuilder MaxOutputDelay(double maxOutputDelay)
        {
            Settings.MaxOutputDelay = maxOutputDelay;
            return this;
        }

        public SynRunnerSettingsBuilder MinOutputDelay(double minOutputDelay)
        {
            Settings.MinOutputDelay = minOutputDelay;
            return this;
        }

        public SynRunnerSettingsBuilder MaxTransition(double maxTransition)
        {
            Settings.MaxTransition = maxTransition;
            return this;
        }

        public SynRunnerSettingsBuilder MaxArea(double maxArea)
        {
            Settings.MaxArea = maxArea;
            return this;
        }

        public SynRunnerSettingsBuilder CapacitanceFactor(int capacitanceFactor)
        {
            Settings.CapacitanceFactor = capacitanceFactor;
            return this;
        }

        public SynRunnerSettingsBuilder LoadFactor(int loadFactor)
        {
            Settings.LoadFactor = loadFactor;
            return this;
        }

        public SynRunnerSettingsBuilder MaxFanout(int maxFanout)
        {
            Settings.MaxFanout = maxFanout;
            return this;
        }

        public SynRunnerSettingsBuilder CriticalRange(int criticalRange)
        {
            Settings.CriticalRange = criticalRange;
            return this;
        }

        public SynRunnerSettingsBuilder TimingReportNumber(int timingReportNumber)
        {
            Settings.TimingReportNumber = timingReportNumber;
            return this;
        }

        public SynRunnerSettingsBuilder Port(Action<PortSettingsBuilder> action)
        {
            PortSettingsBuilder builder = new PortSettingsBuilder();
            action?.Invoke(builder);
            Settings.PortSettings = builder.PortSettings;
            return this;
        }
    }

    public class PortSettingsBuilder
    {
        public List<PortSetting> PortSettings { get; set; } = new List<PortSetting>();

        public PortSettingsBuilder Port(string name, string loadOf)
        {
            PortSettings.Add(new PortSetting { Name = name, LoadOf = loadOf });
            return this;
        }

        public PortSettingsBuilder Port(string name, int loadfactor = 5)
        {
            PortSettings.Add(new PortSetting {Name = name, LoadFactor = loadfactor});
            return this;
        }
    }
}
