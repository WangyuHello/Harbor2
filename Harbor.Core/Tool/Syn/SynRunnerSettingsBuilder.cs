using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Harbor.Common.Project;

namespace Harbor.Core.Tool.Syn
{
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
            PortSettings.Add(new PortSetting { Name = name, LoadFactor = loadfactor });
            return this;
        }
    }
}
