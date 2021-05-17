using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;

namespace Harbor.Python.Tool
{
    public static class ConvertAMS
    {
        const string Banner = @"
// Created by: Harbor
// Version   : 2.0.0
// Time      : ";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="top"></param>
        /// <param name="source">RTL源代码</param>
        /// <param name="output">输出文件名称, 顶层模块不变, 其余模块重命名为 _AMS</param>
        public static void Run(string top, string source, string output, string workingDirectory)
        {
            PythonHelper.SetEnvironment(workingDirectory, () =>
            {
                dynamic pyverilog = Py.Import("pyverilog");
                dynamic parser = Py.Import("pyverilog.vparser.parser");
                dynamic vast = pyverilog.vparser.ast;
                dynamic codegen = Py.Import("pyverilog.ast_code_generator.codegen");

                void ConvertRtlToAms(dynamic node)
                {
                    foreach (var i in node.children())
                    {
                        switch (i.__class__.__name__.As<string>())
                        {
                            case "ModuleDef":
                                var moduleName = i.name.As<string>();
                                if (moduleName != top)
                                {
                                    i.name = moduleName + "_AMS";
                                }

                                break;
                            case "Instance":
                            case "InstanceList":
                                var n = i.module.As<string>();
                                i.module = n + "_AMS";
                                break;
                        }

                        ConvertRtlToAms(i);
                    }
                }

                dynamic srcTuple = parser.parse(new List<string> {source});
                dynamic srcAst = srcTuple[0];

                ConvertRtlToAms(srcAst);

                dynamic codegenI = codegen.ASTCodeGenerator();
                string rslt = codegenI.visit(srcAst).As<string>();

                File.WriteAllText(output, Banner + DateTime.Now + Environment.NewLine + rslt);
            });
        }

        public static void Run2(string top, string source, string output, string workingDirectory)
        {
            var names = MethodBase.GetCurrentMethod()?.DeclaringType?.Namespace;
            var code = PythonHelper.GetCodeFromResource(names + ".ConvertAMS.py");

            string rslt = "";
            PythonHelper.SetEnvironment(workingDirectory, () =>
            {
                using var scope = Py.CreateScope();
                scope.Set("filename", source);
                scope.Set("top", top);
                scope.Exec(code);
                rslt = scope.Get<string>("rslt");
            });
            File.WriteAllText(output, Banner + DateTime.Now + Environment.NewLine + rslt, new UTF8Encoding(false));
        }
    }
}
