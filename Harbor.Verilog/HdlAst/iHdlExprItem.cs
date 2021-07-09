using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Verilog.HdlAst
{
    public class iHdlExprItem : WithPos, iHdlObj
    {
        public CodePosition position { get; set; }
    }
}
