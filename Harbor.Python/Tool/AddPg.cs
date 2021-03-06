using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Harbor.Common.Project;
using Harbor.Common.Util;
using Mono.Unix;
using Python.Runtime;
using Path = System.IO.Path;

namespace Harbor.Python.Tool
{
    public class AddPgSettings : ToolSettings
    {
        public Library Library { get; set; }
        public ProjectInfo ProjectInfo { get; set; }
        public string FileName { get; set; }
        public string Output { get; set; }
        public string[] WireOnlyCells { get; set; }
    }

    public class AddPg : PythonTool<AddPgSettings>
    {
        //public static void Run(Library library, ProjectInfo projectInfo, string filename, string output, string workingDirectory)
        //{
        //    PythonHelper.SetEnvironment(workingDirectory, () =>
        //    {
        //        dynamic pyverilog = Py.Import("pyverilog");
        //        dynamic parser = Py.Import("pyverilog.vparser.parser");
        //        dynamic vast = pyverilog.vparser.ast;
        //        dynamic codegen = Py.Import("pyverilog.ast_code_generator.codegen");

        //        var libInsPowerPin = "VDD";
        //        var libInsGroundPin = "VSS";

        //        if (!string.IsNullOrEmpty(library.PrimaryStdCell.power_pin))
        //        {
        //            libInsPowerPin = library.PrimaryStdCell.power_pin;
        //        }

        //        if (!string.IsNullOrEmpty(library.PrimaryStdCell.ground_pin))
        //        {
        //            libInsGroundPin = library.PrimaryStdCell.ground_pin;
        //        }

        //        var macroPowerPins = GetMacroPowerPins(projectInfo);

        //        void AddPowerForPort(dynamic portlist)
        //        {
        //            dynamic dvdd = vast.Port(name: "DVDD", width: null, dimensions: 1, type: null, lineno: -1);
        //            dynamic dvss = vast.Port(name: "DVSS", width: null, dimensions: 1, type: null, lineno: -1);
        //            dynamic ports = PyList.AsList(portlist.ports);
        //            ports.Append(dvdd);
        //            ports.Append(dvss);
        //            portlist.ports = PyTuple.AsTuple(ports);
        //        }

        //        void AddPowerForDecl(dynamic i)
        //        {
        //            dynamic dvdd = vast.Input("DVDD");
        //            dynamic dvss = vast.Input("DVSS");
        //            var inputs = new List<PyObject> {dvdd, dvss};
        //            dynamic decl = vast.Decl(inputs, lineno: -1);
        //            dynamic items = PyList.AsList(i.items);
        //            items.Insert(0, decl);
        //            i.items = PyTuple.AsTuple(items);
        //        }

        //        void AddPowerForLibInstance(dynamic instance)
        //        {
        //            dynamic ports = PyList.AsList(instance.portlist);
        //            dynamic dvdd = vast.Identifier(name: "DVDD", lineno: -1);
        //            dynamic dvss = vast.Identifier(name: "DVSS", lineno: -1);
        //            PyObject vdd = vast.PortArg(libInsPowerPin, dvdd, lineno: -1);
        //            PyObject vnw = vast.PortArg("VNW", dvdd, lineno: -1);
        //            PyObject vss = vast.PortArg(libInsGroundPin, dvss, lineno: -1);
        //            PyObject vpw = vast.PortArg("VPW", dvss, lineno: -1);

        //            if (library.PrimaryStdCell.Name.StartsWith("scc"))
        //            {
        //                ports.Append(vdd); //SMIC Std Cell 含有VNW/VPW接口
        //                ports.Append(vnw);
        //                ports.Append(vss);
        //                ports.Append(vpw);
        //            }
        //            else
        //            {
        //                ports.Append(vdd);
        //                ports.Append(vss);
        //            }

        //            instance.portlist = PyTuple.AsTuple(ports);
        //        }

        //        void AddPowerForUserInstance(dynamic instance)
        //        {
        //            dynamic ports = PyList.AsList(instance.portlist);
        //            dynamic dvdd = vast.Identifier(name: "DVDD", lineno: -1);
        //            dynamic dvss = vast.Identifier(name: "DVSS", lineno: -1);
        //            PyObject vdd = vast.PortArg("DVDD", dvdd, lineno: -1);
        //            PyObject vss = vast.PortArg("DVSS", dvss, lineno: -1);
        //            ports.Append(vdd);
        //            ports.Append(vss);
        //            instance.portlist = PyTuple.AsTuple(ports);
        //        }

        //        void AddPowerForMacroInstance(dynamic instance)
        //        {
        //            string moduleName = instance.module.As<string>();
        //            var powerPins = macroPowerPins[moduleName]["power_pins"];
        //            var groundPins = macroPowerPins[moduleName]["ground_pins"];

        //            dynamic ports = PyList.AsList(instance.portlist);
        //            dynamic dvdd = vast.Identifier(name: "DVDD", lineno: -1);
        //            dynamic dvss = vast.Identifier(name: "DVSS", lineno: -1);

        //            foreach (var p in powerPins)
        //            {
        //                dynamic p2 = vast.PortArg(p, dvdd, lineno: -1);
        //                ports.Append(p2);
        //            }

        //            foreach (var g in groundPins)
        //            {
        //                dynamic g2 = vast.PortArg(g, dvss, lineno: -1);
        //                ports.Append(g2);
        //            }

        //            instance.portlist = PyTuple.AsTuple(ports);
        //        }

        //        int zeroCounter = 0;

        //        void Convert1b0ToWire(dynamic i)
        //        {
        //            dynamic c1 = i.argname;
        //            string c1Name = c1.__class__.__name__.As<string>();
        //            if (c1Name == "IntConst")
        //            {
        //                if (c1.value.As<string>() == "1'b0")
        //                {
        //                    zeroCounter += 1;
        //                    dynamic hbZero = vast.Identifier("HARBOR_ZERO_" + zeroCounter);
        //                    i.argname = hbZero;
        //                }
        //            }
        //            else if (c1Name == "Concat")
        //            {
        //                PyList l1 = PyList.AsList(c1.list);
        //                for (int j = 0; j < l1.Length(); j++)
        //                {
        //                    dynamic c2 = l1[j];
        //                    string c2Name = c2.__class__.__name__.As<string>();
        //                    if (c2Name == "IntConst" && c2.value.As<string>() == "1'b0")
        //                    {
        //                        zeroCounter += 1;
        //                        dynamic hbZero = vast.Identifier("HARBOR_ZERO_" + zeroCounter);
        //                        l1[j] = hbZero;
        //                    }
        //                }

        //                c1.list = PyTuple.AsTuple(l1);
        //            }
        //        }

        //        var libCellList = library.PrimaryStdCell.GetCellList();

        //        void AddPower(dynamic node)
        //        {
        //            foreach (var i in node.children())
        //            {
        //                var iName = i.__class__.__name__.As<string>();
        //                switch (iName)
        //                {
        //                    case "ModuleDef":
        //                        AddPowerForPort(i.portlist);
        //                        AddPowerForDecl(i);
        //                        break;
        //                    case "Instance":
        //                        string insModuleName = i.module.As<string>();
        //                        if (libCellList.Contains(insModuleName))
        //                        {
        //                            AddPowerForLibInstance(i);
        //                        }
        //                        else if (macroPowerPins.HasKey(insModuleName))
        //                        {
        //                            AddPowerForMacroInstance(i);
        //                        }
        //                        else
        //                        {
        //                            AddPowerForUserInstance(i);
        //                        }

        //                        break;
        //                    case "PortArg":
        //                        Convert1b0ToWire(i);
        //                        break;
        //                }

        //                AddPower(i);
        //            }
        //        }

        //        dynamic srcTuple = parser.parse(new List<string> {filename});
        //        dynamic srcAst = srcTuple[0];

        //        AddPower(srcAst);

        //        dynamic codegenI = codegen.ASTCodeGenerator();
        //        string rslt = codegenI.visit(srcAst).As<string>();
        //        File.WriteAllText(output, PythonHelper.Banner + DateTime.Now + Environment.NewLine + rslt, new UTF8Encoding(false));
        //    });
        //}

        public static void Run2(Library library, ProjectInfo projectInfo, string filename, string output, string[] wireOnlyCells, 
            string workingDirectory)
        {
            var c = new AddPg();
            c.Run(new AddPgSettings
            {
                Library = library,
                ProjectInfo = projectInfo,
                FileName = filename,
                Output = output,
                WireOnlyCells = wireOnlyCells,
                WorkingDirectory = workingDirectory
            });
        }

        public static string GetAbsoluteDirectoryPath(ProjectInfo info, string path)
        {
            if (!UnixPath.IsPathRooted(path))
            {
                var p2 = Path.Combine(info.Directory.FullPath, path);
                var di = new DirectoryInfo(p2);
                return di.FullName;
            }

            return path;
        }

        private static PyDict GetMacroPowerPins(ProjectInfo projectInfo)
        {
            var macroPowerPins = new PyDict();
            if (projectInfo.Reference == null)
            {
                return macroPowerPins;
            }

            foreach (var pref in projectInfo.Reference)
            {
                var name = pref.Name;
                var path = GetAbsoluteDirectoryPath(projectInfo, pref.Path); //相对路径

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

                        var lef = LefObject.Parse(lefFile.FullName);
                        var pins = lef.Macros[name].Pins;
                        var powerPins = new List<string>();
                        var groundPins = new List<string>();
                        foreach (var p in pins)
                        {
                            switch (p.Use)
                            {
                                case "POWER":
                                    powerPins.Add(p.Name);
                                    break;
                                case "GROUND":
                                    groundPins.Add(p.Name);
                                    break;
                            }
                        }

                        var pinDict = new PyDict();
                        pinDict["power_pins"] = powerPins.ToPython();
                        pinDict["ground_pins"] = groundPins.ToPython();
                        macroPowerPins[name] = pinDict;
                        break;
                }
            }
            return macroPowerPins;
        }

        protected override int RunCore(AddPgSettings settings)
        {
            var libInsPowerPin = "VDD";
            var libInsGroundPin = "VSS";

            if (!string.IsNullOrEmpty(settings.Library.PrimaryStdCell.power_pin))
            {
                libInsPowerPin = settings.Library.PrimaryStdCell.power_pin;
            }

            if (!string.IsNullOrEmpty(settings.Library.PrimaryStdCell.ground_pin))
            {
                libInsGroundPin = settings.Library.PrimaryStdCell.ground_pin;
            }

            settings.WireOnlyCells ??= Array.Empty<string>();
            var libCellList = settings.Library.PrimaryStdCell.GetCellList();
            var primaryStdcellName = settings.Library.PrimaryStdCell.Name;

            using var scope = Py.CreateScope(GetType().FullName);
            var macroPowerPins = GetMacroPowerPins(settings.ProjectInfo);

            scope.Set("filename", settings.FileName);
            scope.Set("lib_ins_power_pin", libInsPowerPin);
            scope.Set("lib_ins_ground_pin", libInsGroundPin);
            scope.Set("macro_power_pins", macroPowerPins);
            scope.Set("lib_ins_list", PyList.AsList(libCellList.ToPython()));
            scope.Set("primary_stdcell_name", primaryStdcellName);
            scope.Set("wire_only_cells", PyList.AsList(settings.WireOnlyCells.ToPython()));
            scope.Exec(Code);
            var rslt = scope.Get<string>("rslt");
            File.WriteAllText(settings.Output, Banner + DateTime.Now + Environment.NewLine + rslt, new UTF8Encoding(false));
            return 0;
        }
    }
}
