using System;
using System.Collections.Generic;
using System.Text;
using Harbor.Common.Project;

namespace Harbor.Core.Tool.Syn.Model
{
    public class BuildTclModel : HarborTextModel
    {
        public string ScriptRootPath { get; set; }
        public string WorkPath { get; set; }
        public string LibPath { get; set; }
        public string RptPath { get; set; }
        public string NetPath { get; set; }
        public string SvfPath { get; set; }
        public string ObjPath { get; set; }
        public string MemPath { get; set; }
        public int Cores { get; set; }
        public string LibName { get; set; }
        public string LibFullName { get; set; }
        public string TopName { get; set; }
        public List<string> SourceFullPaths { get; set; } = new();
        public List<string> AdditionalTimingDbPaths { get; set; } = new();
        public List<string> IOTimingDbPaths { get; set; } = new();
        public string ClkName { get; set; }
        public string RstName { get; set; }
        public double ClkPeriod { get; set; }
        public double ClkLatency { get; set; }
        public double ClkUncertainty { get; set; }
        public double MaxInputDelay { get; set; }
        public double MinInputDelay { get; set; }
        public double MaxOutputDelay { get; set; }
        public double MinOutputDelay { get; set; }
        public string InvName { get; set; }
        public string InvPortName { get; set; }
        public int CapFactor { get; set; }
        public int LoadFactor { get; set; }
        public int MaxFanout { get; set; }
        public double MaxTransition { get; set; }
        public double MaxArea { get; set; }
        public int CriticalRange { get; set; }
        public bool UseCompileUltra { get; set; }
        public int TimingRptNum { get; set; }
        public bool AllowTriState { get; set; }

        public Library.LibraryStdCell StdCell { get; set; }

        public List<PortSetting> PortSettings { get; set; }
    }
}
