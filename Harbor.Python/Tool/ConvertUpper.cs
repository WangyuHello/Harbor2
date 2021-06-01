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
    public static class ConvertUpper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="top"></param>
        /// <param name="source">RTL源代码</param>
        /// <param name="netlist">LAYOUT后输出的网表,将会提取其端口顺序</param>
        /// <param name="output">输出文件名称,输出文件的端口将会全部大写,并且添加DVDD和DVSS占位,并且端口顺序和netlist.v相同</param>
        public static void Run(string top, string source, string netlist, string output, string workingDirectory)
        {
            PythonHelper.SetEnvironment(workingDirectory, () =>
            {
                dynamic pyverilog = Py.Import("pyverilog");
                dynamic parser = Py.Import("pyverilog.vparser.parser");
                dynamic vast = pyverilog.vparser.ast;
                dynamic codegen = Py.Import("pyverilog.ast_code_generator.codegen");

                void GetPorts(dynamic node, Dictionary<string, List<string>> portLists)
                {
                    foreach (var i in node.children())
                    {
                        var iName = i.__class__.__name__.As<string>();
                        if (iName == "ModuleDef")
                        {
                            var moduleName = i.name.As<string>();
                            var realPortList = new List<string>();
                            foreach (var p in i.portlist.ports)
                            {
                                realPortList.Add(p.__class__.__name__.As<string>() == "Ioport"
                                    ? (string) p.first.name
                                    : (string) p.name);
                            }

                            portLists.Add(moduleName, realPortList);
                        }

                        GetPorts(i, portLists);
                    }
                }

                void ConvertPortsToUpperInFunction(dynamic node)
                {
                    foreach (var i in node.children())
                    {
                        if (i.__class__.__name__.As<string>() == "Input")
                        {
                            string n = i.name.ToString();
                            i.name = n.ToUpper();
                        }

                        ConvertPortsToUpperInFunction(i);
                    }
                }

                void ConvertIdentifierToUpperInModule(List<string> srcps, dynamic node)
                {
                    foreach (var i in node.children())
                    {
                        if (i.__class__.__name__.As<string>() == "Identifier")
                        {
                            string n = i.name.ToString();
                            if (srcps.Contains(n))
                            {
                                i.name = n.ToUpper();
                            }
                        }

                        ConvertIdentifierToUpperInModule(srcps, i);
                    }
                }

                void ConvertPortsToUpper(Dictionary<string, List<string>> srcps,
                    Dictionary<string, List<string>> netlistps, dynamic node)
                {
                    foreach (var i in node.children())
                    {
                        var iName = i.__class__.__name__.As<string>();
                        if (iName == "ModuleDef")
                        {
                            //仅修改模块的Port, 并且根据netlist调整Port顺序
                            var moduleName = i.name.As<string>();
                            if (moduleName == top)
                            {
                                dynamic portList = i.portlist.ports;
                                var newOrderPortList = new List<PyObject>();
                                if (netlistps.ContainsKey(moduleName)) // 如果netlist含有这个模块,则调整顺序并大写
                                {
                                    foreach (string targetOrderedPort in netlistps[moduleName])
                                    {
                                        foreach (var originOrderedPort in portList)
                                        {
                                            string originOrderedPortName;
                                            if (originOrderedPort.__class__.__name__.As<string>() == "Ioport")
                                            {
                                                originOrderedPortName = originOrderedPort.first.name.As<string>();
                                                originOrderedPortName = originOrderedPortName.ToUpper();
                                                originOrderedPort.first.name = originOrderedPortName;
                                            }
                                            else
                                            {
                                                originOrderedPortName = originOrderedPort.name.As<string>();
                                                originOrderedPortName = originOrderedPortName.ToUpper();
                                                originOrderedPort.first.name = originOrderedPortName;
                                            }

                                            if (originOrderedPortName == targetOrderedPort)
                                            {
                                                newOrderPortList.Add(originOrderedPort);
                                            }
                                        }
                                    }
                                }
                                else //如果netlist不不包含这个模块,则仅大写
                                {
                                    foreach (var p in portList)
                                    {
                                        if (p.__class__.__name__.As<string>() == "Ioport")
                                        {
                                            string n = p.first.name.As<string>();
                                            p.first.name = n.ToUpper();
                                        }
                                        else
                                        {
                                            string n = p.name.As<string>();
                                            p.first.name = n.ToUpper();
                                        }

                                        newOrderPortList.Add(p);
                                    }
                                }

                                dynamic dvdd = vast.Ioport(first: vast.Input(name: "DVDD"), lineno: -1);
                                dynamic dvss = vast.Ioport(first: vast.Input(name: "DVSS"), lineno: -1);

                                newOrderPortList.Add(dvdd);
                                newOrderPortList.Add(dvss);
                                i.portlist.ports = newOrderPortList;

                                ConvertIdentifierToUpperInModule(srcps[top], i);
                            }
                        }

                        ConvertPortsToUpper(srcps, netlistps, i);
                    }
                }

                dynamic srcTuple = parser.parse(new List<string> {source});
                dynamic srcAst = srcTuple[0];
                dynamic netlistTuple = parser.parse(new List<string> {netlist});
                dynamic netlistAst = netlistTuple[0];

                var srcPorts = new Dictionary<string, List<string>>();
                var netlistPorts = new Dictionary<string, List<string>>();
                GetPorts(srcAst, srcPorts);
                GetPorts(netlistAst, netlistPorts);
                ConvertPortsToUpper(srcPorts, netlistPorts, srcAst);

                dynamic codegenI = codegen.ASTCodeGenerator();
                string rslt = codegenI.visit(srcAst).As<string>();

                File.WriteAllText(output, PythonHelper.Banner + DateTime.Now + Environment.NewLine + rslt);
            });
        }

        public static void Run2(string top, string source, string netlist, string output, string workingDirectory)
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.FullName;
            var code = PythonHelper.GetCodeFromResource($"{className}.py");

            string rslt = "";
            PythonHelper.SetEnvironment(workingDirectory, () =>
            {
                using var scope = Py.CreateScope();
                scope.Set("source", source);
                scope.Set("netlist", netlist);
                scope.Set("top", top);
                scope.Exec(code);
                rslt = scope.Get<string>("rslt");
            });
            File.WriteAllText(output, PythonHelper.Banner + DateTime.Now + Environment.NewLine + rslt, new UTF8Encoding(false));
        }
    }
}
