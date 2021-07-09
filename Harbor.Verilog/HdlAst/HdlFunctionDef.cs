using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Verilog.HdlAst
{
    public class HdlFunctionDef : HdlIdDef
    {
        public iHdlExprItem returnT;
        public List<HdlIdDef> @params;
        public List<iHdlObj> body;
        bool is_operator; 
        bool is_virtual;
        bool is_task;
        bool is_declaration_only;

        public HdlFunctionDef(string _name, bool _is_operator, iHdlExprItem _returnT, List<HdlIdDef> _params)
            : base(_name, null, null)
        {
            returnT = _returnT;
            @params = _params;
            is_operator = _is_operator;
            is_declaration_only = true;
            @params ??= new List<HdlIdDef>();
        }
    }
}
