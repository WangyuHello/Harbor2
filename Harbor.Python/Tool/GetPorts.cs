using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.Tooling;
using Python.Runtime;

namespace Harbor.Python.Tool
{
    public class VerilogPortDefinition
    {
        public string Name { get; set; }
        public (int msb, int lsb) Width { get; set; }
    }

    public class GetPortsSettings : ToolSettings
    {
        public string FileName { get; set; }
        public string TopModuleName { get; set; }
    }

    public class GetPorts : PythonTool<GetPortsSettings, List<VerilogPortDefinition>>
    {
        public static List<VerilogPortDefinition> Run(string filename, string topModuleName, string workingDirectory)
        {
            List<VerilogPortDefinition> topPorts = new List<VerilogPortDefinition>();
            PythonHelper.SetEnvironment(workingDirectory, () =>
            {
                dynamic pyverilog = Py.Import("pyverilog");
                dynamic parser = Py.Import("pyverilog.vparser.parser");
                dynamic vast = pyverilog.vparser.ast;

                void GetPortsInPorts(dynamic portlist)
                {
                    foreach (var p in portlist.ports)
                    {
                        if (p.__class__.ToString() == vast.Ioport.ToString())
                        {
                            if (p.first.width != null)
                            {
                                var w = p.first.width;
                                topPorts.Add(new VerilogPortDefinition
                                {
                                    Name = p.first.name,
                                    Width = (int.Parse(w.msb.value.As<string>()), int.Parse(w.lsb.value.As<string>()))
                                });
                            }
                            else
                            {
                                topPorts.Add(new VerilogPortDefinition
                                    {Name = p.first.name, Width = (0, 0)});
                            }
                        }
                    }
                }

                void GetPortsInItems(dynamic items)
                {
                    foreach (var i in items)
                    {
                        if (i.__class__.ToString() == vast.Decl.ToString())
                        {
                            foreach (var ll in i.list)
                            {
                                if (ll.__class__.ToString() == vast.Input.ToString() ||
                                    ll.__class__.ToString() == vast.Output.ToString())
                                {
                                    if (ll.width != null)
                                    {
                                        var w = ll.width;
                                        topPorts.Add(new VerilogPortDefinition
                                        {
                                            Name = ll.name,
                                            Width = (int.Parse(w.msb.value.As<string>()),
                                                int.Parse(w.lsb.value.As<string>()))
                                        });
                                    }
                                    else
                                    {
                                        topPorts.Add(new VerilogPortDefinition
                                            {Name = ll.name, Width = (0, 0)});
                                    }
                                }
                            }
                        }
                    }
                }

                void GetPortsInner(dynamic node)
                {
                    foreach (var i in node.children())
                    {
                        dynamic iName = i.__class__.__name__.ToString();
                        if (iName == "ModuleDef" && i.name.ToString() == topModuleName)
                        {
                            GetPortsInPorts(i.portlist);
                            GetPortsInItems(i.items);
                        }

                        GetPortsInner(i);
                    }
                }


                dynamic tuple = parser.parse(new List<string> {filename});
                dynamic ast = tuple[0];
                GetPortsInner(ast);
            });
            return topPorts;
        }

        public static List<VerilogPortDefinition> Run2(string filename, string topModuleName, string workingDirectory)
        {
            var c = new GetPorts();
            return c.Run(new GetPortsSettings
            {
                FileName = filename,
                TopModuleName = topModuleName,
                WorkingDirectory = workingDirectory
            });
        }

        protected override List<VerilogPortDefinition> RunCore(GetPortsSettings settings)
        {
            var rslt = new List<VerilogPortDefinition>();
            using var scope = Py.CreateScope(GetType().FullName);
            scope.Set("filename", settings.FileName);
            scope.Set("top", settings.TopModuleName);
            scope.Exec(Code);
            var r = scope.Get<PyObject>("top_ports");
            dynamic r2 = PyList.AsList(r);
            foreach (var i in r2)
            {
                rslt.Add(new VerilogPortDefinition
                {
                    Name = i["Name"],
                    Width = (i["Width"]["msb"], i["Width"]["lsb"])
                });
            }
            return rslt;
        }
    }
}
