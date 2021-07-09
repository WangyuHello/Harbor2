using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Verilog.HdlAst
{
    public class HdlCompInst
    {
        public iHdlExprItem name;
        public iHdlExprItem module_name;

        public List<iHdlExprItem> genericMap;
        public List<iHdlExprItem> portMap;

        public HdlCompInst(iHdlExprItem _name, iHdlExprItem _module_name)
        {
            module_name = _module_name;
            name = _name;
        }
    }
}
