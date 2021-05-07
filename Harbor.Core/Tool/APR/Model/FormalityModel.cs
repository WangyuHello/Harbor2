using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.APR.Model
{
    public class FormalityModel
    {
        public string LibPath { get; set; }
        public string LibName { get; set; }
        public string LibFullName { get; set; }
        public string TopName { get; set; }
        public string Netlist { get; set; }
        public string SynNetlist { get; set; }
        public string ScriptRootPath { get; set; }
        public List<string> AdditionalTimingDbPaths { get; set; } = new List<string>();
        public List<string> IOTimingDbPaths { get; set; } = new List<string>();
    }
}
