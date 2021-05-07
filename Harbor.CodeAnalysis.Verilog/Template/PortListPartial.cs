using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.CodeAnalysis.Verilog.Template
{
    public partial class PortList
    {
        public List<string> Ports { get; set; } = new List<string>();

        public string ToLfString()
        {
            var t = TransformText();
            return t.Replace("\r", "");
        }
    }
}
