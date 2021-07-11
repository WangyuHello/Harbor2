using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Verilog.HdlAst
{
    public class HdlValueIdspace : WithNameAndDoc, iHdlObj
    {
        public bool defs_only; // true for VHDL package, false for VHDL package body
        public List<iHdlObj> objs;

        public HdlValueIdspace() : base() 
        {
            defs_only = false;
        }
    }
}
