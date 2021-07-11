using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Verilog.HdlAst
{
    public class HdlModuleDef : WithNameAndDoc, iHdlObj
    {
        public iHdlExprItem module_name;
        public HdlModuleDec dec;
        public List<iHdlObj> objs;
    }
}
