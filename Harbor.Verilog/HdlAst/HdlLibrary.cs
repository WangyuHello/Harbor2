using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Verilog.HdlAst
{
    public class HdlLibrary : WithNameAndDoc, iHdlObj
    {
        public HdlLibrary(string name) : base(name) { }
    }
}
