using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;

namespace Harbor.Core.Tool.GetPorts
{
    public static class GetPorts
    {
        public static void Run(string filename)
        {
            using (Py.GIL())
            {
                dynamic pyverilog = Py.Import("pyverilog");
                dynamic parser = pyverilog.vparser.parser;
                dynamic vast = pyverilog.vparser.ast;
                dynamic ParserError = pyverilog.vparser.plyparser.ParseError;

                dynamic ast = parser.parse(new List<string> {filename});
                GetPortsInner(ast);
            }
        }

        private static void GetPortsInner(dynamic node)
        {
            foreach (var i in node.children())
            {
                dynamic i_name = i.__class__.__name__;
                if (i_name == "ModuleDef")
                {
                    
                }
            }
        }
    }
}
