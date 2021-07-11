using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Harbor.Verilog.HdlAst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Verilog
{
    internal static class CreateObjectHelper
    {
        internal static T update_code_position<T>(T @object, IParseTree node)
        {
            if (node != null)
            {
                var ctx = node as ParserRuleContext;
                if (ctx == null)
                {
                    if (node is ITerminalNode tn && tn.Parent != null)
                    {
                        ctx = tn.Parent as ParserRuleContext;
                    }
                }

                if (ctx != null)
                {
                    if (@object is WithPos wp)
                    {
                        wp.position.update_from_elem(ctx);
                    }
                }
            }
            return @object;
        }

        //internal T create_object<T, Args>(IParseTree node, Args args)
        //{

        //}
    }
}
