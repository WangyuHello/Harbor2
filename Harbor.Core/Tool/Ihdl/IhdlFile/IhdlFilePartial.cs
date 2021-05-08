using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Core.Tool.Ihdl.IhdlFile
{
    public partial class IhdlFile
    {
        public string DestSchLib { get; set; }
        public List<string> RefLibList { get; set; }
        public string TopCellName { get; set; }
    }
}
