using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Verilog.HdlAst
{
    public class HdlModuleDec : WithNameAndDoc, iHdlObj
    {
        public List<HdlIdDef> generics;
        public List<HdlIdDef> ports;
        // vhdl entity declarative items
        public List<HdlIdDef> objs;

        public HdlModuleDec() : base() { }

        public HdlIdDef getPortByName(string name)
        {
            return ports.FirstOrDefault(p => p.name == name);
        }
    }
}
