using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.CodeAnalysis.Verilog.Template
{
    public partial class ModuleDeclaration
    {
        public string ModuleName { get; set; } = "";
        public string ParamList { get; set; } = "";
        public string PortList { get; set; } = "";
        public List<string> Items { get; set; } = new List<string>();

        public string ToLfString()
        {
            var t = TransformText();
            return t.Replace("\r", "");
        }
    }
}
