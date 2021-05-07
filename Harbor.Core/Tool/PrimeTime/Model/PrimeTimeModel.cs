using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.PrimeTime.Model
{
    public class PrimeTimeModel
    {
        public string LibPath { get; set; }
        public string LibName { get; set; }
        public string LibFullName { get; set; }
        public string TopName { get; set; }
        public bool APRorSyn { get; set; }
        public string APRNetlist { get; set; }
        public string SynNetlist { get; set; }
        public string ScriptRootPath { get; set; }
        public int Cores { get; set; }
        public string RptPath { get; set; }
        public double? ClkUncertaintySetup { get; set; }
        public double? ClkUncertaintyHold { get; set; }
        public double? MaxTransition { get; set; }
        public string ClkName { get; set; }
    }
}
