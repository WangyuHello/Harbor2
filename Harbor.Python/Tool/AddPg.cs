using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harbor.Common.Project;
using Newtonsoft.Json.Linq;
using Python.Runtime;

namespace Harbor.Python.Tool
{
    public static class AddPg
    {
        const string Banner = @"
// Created by: Harbor
// Version   : 2.0.0
// Author    : Wang Yu
";

        public static void Run(Library library, ProjectInfo projectInfo, string filename)
        {
            PythonHelper.SetEnvironment();
            using (Py.GIL())
            {
                dynamic pyverilog = Py.Import("pyverilog");
                dynamic parser = Py.Import("pyverilog.vparser.parser");
                dynamic vast = pyverilog.vparser.ast;
                dynamic codegen = Py.Import("pyverilog.ast_code_generator.codegen");

                var libInsPowerPin = "VDD";
                var libInsGroundPin = "VSS";

                if (!string.IsNullOrEmpty(library.PrimaryStdCell.power_pin))
                {
                    libInsPowerPin = library.PrimaryStdCell.power_pin;
                }

                if (!string.IsNullOrEmpty(library.PrimaryStdCell.ground_pin))
                {
                    libInsGroundPin = library.PrimaryStdCell.ground_pin;
                }

                var macroPowerPins = GetMacroPowerPins(projectInfo);

                void AddPowerForPort(dynamic portlist)
                {
                    dynamic dvdd = vast.Port(name: "DVDD", width: null, dimensions: 1, type: null, lineno: -1);
                    dynamic dvss = vast.Port(name: "DVSS", width: null, dimensions: 1, type: null, lineno: -1);
                    List<PyObject> ports = portlist.ports.As<List<PyObject>>();
                    ports.Add(dvdd);
                    ports.Add(dvss);
                    portlist.ports = ports;
                }

                void AddPowerForDecl(dynamic i)
                {
                    dynamic dvdd = vast.Input("DVDD");
                    dynamic dvss = vast.Input("DVSS");
                    var inputs = new List<PyObject> {dvdd, dvss};
                    dynamic decl = vast.Decl(inputs, lineno: -1);
                    dynamic items = i.items;
                    items.insert(0, decl);
                    i.items = items;
                }

                void AddPowerForLibInstance(dynamic instance)
                {
                    List<PyObject> ports = instance.portlist.As<List<PyObject>>();
                    dynamic dvdd = vast.Identifier(name: "DVDD", lineno: -1);
                    dynamic dvss = vast.Identifier(name: "DVSS", lineno: -1);
                    PyObject vdd = vast.PortArg(libInsPowerPin, dvdd, lineno: -1);
                    PyObject vnw = vast.PortArg("VNW", dvdd, lineno: -1);
                    PyObject vss = vast.PortArg(libInsGroundPin, dvss, lineno: -1);
                    PyObject vpw = vast.PortArg("VPW", dvss, lineno: -1);

                    ports.AddRange(library.PrimaryStdCell.Name.StartsWith("scc")
                        ? new[] {vdd, vnw, vss, vpw } //SMIC Std Cell 含有VNW/VPW接口
                        : new[] {vdd, vss});

                    instance.portlist = ports;
                }

                void AddPowerForUserInstance(dynamic instance)
                {
                    List<PyObject> ports = instance.portlist.As<List<PyObject>>();
                    dynamic dvdd = vast.Identifier(name: "DVDD", lineno: -1);
                    dynamic dvss = vast.Identifier(name: "DVSS", lineno: -1);
                    PyObject vdd = vast.PortArg("DVDD", dvdd, lineno: -1);
                    PyObject vss = vast.PortArg("DVSS", dvss, lineno: -1);
                    ports.AddRange(new[] { vdd, vss });
                    instance.portlist = ports;
                }

                void AddPowerForMacroInstance(dynamic instance)
                {
                    string moduleName = instance.module.As<string>();
                    var powerPins = macroPowerPins[moduleName].powerPins;
                    var groundPins = macroPowerPins[moduleName].groundPins;

                    List<PyObject> ports = instance.portlist.As<List<PyObject>>();
                    dynamic dvdd = vast.Identifier(name: "DVDD", lineno: -1);
                    dynamic dvss = vast.Identifier(name: "DVSS", lineno: -1);

                    foreach (var p in powerPins)
                    {
                        PyObject p2 = vast.PortArg(p, dvdd, lineno: -1);
                        ports.Add(p2);
                    }

                    foreach (var g in groundPins)
                    {
                        PyObject g2 = vast.PortArg(g, dvdd, lineno: -1);
                        ports.Add(g2);
                    }
                    instance.portlist = ports;
                }

                int zeroCounter = 0;
                void Convert1b0ToWire(dynamic i)
                {
                    dynamic c1 = i.argname;
                    string c1Name = c1.__class__.__name__.As<string>();
                    if (c1Name == "IntConst")
                    {
                        if (c1.value.As<string>() == "1'b0")
                        {
                            zeroCounter += 1;
                            dynamic hbZero = vast.Identifier("HARBOR_ZERO_" + zeroCounter);
                            i.argname = hbZero;
                        }
                    }
                    else if (c1Name == "Concat")
                    {
                        List<PyObject> l1 = c1.list.As<List<PyObject>>();
                        for (int j = 0; j < l1.Count; j++)
                        {
                            dynamic c2 = l1[j];
                            string c2Name = c2.__class__.__name__.As<string>();
                            if (c2Name == "IntConst" && c2.value.As<string>() == "1'b0")
                            {
                                zeroCounter += 1;
                                dynamic hbZero = vast.Identifier("HARBOR_ZERO_" + zeroCounter);
                                l1[j] = hbZero;
                            }
                        }

                        c1.list = l1;
                    }
                }

                var libCellList = library.PrimaryStdCell.GetCellList();

                void AddPower(dynamic node)
                {
                    foreach (var i in node.children())
                    {
                        var iName = i.__class__.__name__.As<string>();
                        switch (iName)
                        {
                            case "ModuleDef":
                                AddPowerForPort(i.portlist);
                                AddPowerForDecl(i);
                                break;
                            case "Instance":
                                string insModuleName = i.module.As<string>();
                                if (libCellList.Contains(insModuleName))
                                {
                                    AddPowerForLibInstance(i);
                                }
                                else if (macroPowerPins.ContainsKey(insModuleName))
                                {
                                    AddPowerForMacroInstance(i);
                                }
                                else
                                {
                                    AddPowerForUserInstance(i);
                                }
                                break;
                            case "PortArg":
                                Convert1b0ToWire(i);
                                break;
                        }
                        AddPower(i);
                    }
                }

                dynamic srcTuple = parser.parse(new List<string> { filename });
                dynamic srcAst = srcTuple[0];

                AddPower(srcAst);

                dynamic codegenI = codegen.ASTCodeGenerator();
                string rslt = codegenI.visit(srcAst).As<string>();
                var fi = new FileInfo(filename);
                File.WriteAllText(Path.Combine(fi.DirectoryName, fi.Name + "_PG", fi.Extension), Banner + Environment.NewLine + rslt);
            }
        }

        private static Dictionary<string, (List<string> powerPins, List<string> groundPins)> GetMacroPowerPins(
            ProjectInfo projectInfo)
        {
            var macroPowerPins = new Dictionary<string, (List<string> powerPins, List<string> groundPins)>();
            if (projectInfo.Reference == null)
            {
                return macroPowerPins;
            }

            foreach (var pref in projectInfo.Reference)
            {
                var name = pref.Name;
                var path = pref.Path;

                var refProjectInfo = ProjectInfo.ReadFromDirectory(path);
                switch (refProjectInfo.Type)
                {
                    case ProjectType.Memory:
                        var lefDir = Path.Combine(path, "lef");
                        var lefDirInfo = new DirectoryInfo(lefDir);
                        var lefFile = lefDirInfo.GetFiles("*.lef").FirstOrDefault();
                        if (lefFile == null)
                        {
                            throw new FileNotFoundException("未找到lef文件: " + name);
                        }

                        var lef = ReadLef.Run(lefFile.FullName);
                        var pins = lef["macro_dict"][name]["info"]["PIN"];
                        var powerPins = new List<string>();
                        var groundPins = new List<string>();
                        foreach (var p in pins)
                        {
                            if (p["info"]["USE"].Value<string>() == "POWER")
                            {
                                powerPins.Add(p["name"].Value<string>());
                            }
                            else if (p["info"]["USE"].Value<string>() == "GROUND")
                            {
                                groundPins.Add(p["name"].Value<string>());
                            }
                        }
                        macroPowerPins.Add(name, (powerPins, groundPins));
                        break;
                }
            }
            return macroPowerPins;
        }
    }
}
