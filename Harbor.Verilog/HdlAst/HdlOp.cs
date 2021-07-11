using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Verilog.HdlAst
{
    public class HdlOp : iHdlExprItem
    {
        public HdlOp() 
        {
            op = HdlOpType.ARROW;
            //operands.reserve(2);
            //operands = new List<iHdlExprItem>(2);
        }

        public HdlOpType op;
        public List<iHdlExprItem> operands;

        public HdlOp(HdlOp o)
        {
            operands = new List<iHdlExprItem>(o.operands);
            op = o.op;
        }

        public HdlOp(HdlOpType operatorType, iHdlExprItem op0)
        {
            operands = new List<iHdlExprItem>
            {
                op0
            };
            op = operatorType;
        }

        public HdlOp(iHdlExprItem op0, HdlOpType operatorType, iHdlExprItem op1)
        {
            if (op0 != null)
            {
                operands = new List<iHdlExprItem>
                {
                    op0
                };
            }
            if (op1 != null)
            {
                operands?.Add(op1);
                if(operands == null) operands = new List<iHdlExprItem>
                {
                    op1
                };
            }
            op = operatorType;
        }

        public static HdlOp index(iHdlExprItem fn, List<iHdlExprItem> operands)
        {
            var res = call(fn, operands);
            res.op = HdlOpType.INDEX;
            return res;
        }
        
        public static HdlOp call(iHdlExprItem fn, List<iHdlExprItem> operands)
        {
            var o = new HdlOp
            {
                op = HdlOpType.CALL
            };
            //o->operands.reserve(operands.size() + 1);
            o.operands.Add(fn);
            o.operands.AddRange(operands);
            return o;
        }

        public static HdlOp parametrization(iHdlExprItem fn, List<iHdlExprItem> operands)
        {
            var o = new HdlOp
            {
                op = HdlOpType.PARAMETRIZATION
            };
            //o->operands.reserve(operands.size() + 1);
            o.operands.Add(fn);
            o.operands.AddRange(operands);
            return o;
        }

        public static HdlOp ternary(iHdlExprItem cond, iHdlExprItem ifTrue, iHdlExprItem ifFalse)
        {
            var o = new HdlOp
            {
                op = HdlOpType.TERNARY
            };
            o.operands.Add(cond);
            o.operands.Add(ifTrue);
            if (ifFalse != null)
            {
                o.operands.Add(ifFalse);
            }
            return o;
        }

        public static HdlOp ternary(ParserRuleContext ctx, iHdlExprItem cond, iHdlExprItem ifTrue, iHdlExprItem ifFalse)
        {
            return CreateObjectHelper.update_code_position(ternary(cond, ifTrue, ifFalse), ctx);
        }

        public static HdlOp call(ParserRuleContext ctx, iHdlExprItem fnId, List<iHdlExprItem> args)
        {
            return CreateObjectHelper.update_code_position(call(fnId, args), ctx);
        }

        public static HdlOp index(ParserRuleContext ctx, iHdlExprItem fnId, List<iHdlExprItem> args)
        {
            return CreateObjectHelper.update_code_position(index(fnId, args), ctx);
        }

        public static HdlOp parametrization(ParserRuleContext ctx, iHdlExprItem fnId, List<iHdlExprItem> args)
        {
            return CreateObjectHelper.update_code_position(parametrization(fnId, args), ctx);
        }


    }
}
