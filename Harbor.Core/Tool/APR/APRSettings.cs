using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Harbor.Core.Tool.APR.Model;
using Harbor.Core.Tool.APR.Tcl;
using Harbor.Core;
using Harbor.Core.Tool.ICC;
using Harbor.Core.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Harbor.Core.Tool.APR
{
    public class RouteSettings
    {
        
    }

    public class PlaceSettings
    {
        public List<MacroPlaceSettings> MacroPlaceSettings { get; set; } = new List<MacroPlaceSettings>();
    }

    public class PinGroupSettings
    {
        /// <summary>
        /// 如果order为-1则按List中的顺序
        /// </summary>
        public List<(string name, int order)> Ports { get; set; } = new List<(string name, int order)>();
        public PortPosition Position { get; set; }
        public decimal Offset { get; set; }
        public int Order { get; set; }
        public decimal PinSpace { get; set; }
    }

    public class PinSettings
    {

        public List<Port> LeftPorts { get; set; } = new List<Port>();
        public List<Port> TopPorts { get; set; } = new List<Port>();
        public List<Port> RightPorts { get; set; } = new List<Port>();
        public List<Port> BottomPorts { get; set; } = new List<Port>();
        public decimal PinSpace { get; set; }
        public PinPlaceMode PinPlaceMode { get; set; } = PinPlaceMode.Uniform;
        /// <summary>
        /// 如果指定了pin约束脚本,则直接引用,不自动生成约束
        /// </summary>
        public FilePath ConstraintFile { get; set; }

        /// <summary>
        /// 所有的用户设置的Pin信息
        /// 如果order为-1则用户未指定顺序
        /// </summary>
        public List<(string name, PortPosition position, int order)> PinPair { get; set; } = new List<(string name, PortPosition position, int order)>();
        public List<PinGroupSettings> Groups = new List<PinGroupSettings>();

        /// <summary>
        /// 从 Verilog 源文件中读取所有的Port，然后根据用户设置好的PinPair变量，将Ports数组填充
        /// </summary>
        /// <param name="synProjectPath"></param>
        /// <param name="m1direction"></param>
        /// <param name="top"></param>
        public void InflatePorts(DirectoryPath synProjectPath, string m1direction, string top)
        {
            var content = File.ReadAllText(synProjectPath.Combine("netlist").CombineWithFilePath($"{top}.ports").FullPath);
            var ports = JsonConvert.DeserializeObject<Port[]>(content);
            var r = new Random(10);

            var leftOrders = new HashSet<int>();
            var topOrders = new HashSet<int>();
            var rightOrders = new HashSet<int>();
            var bottomOrders = new HashSet<int>();

            // 将port放到指定的方位
            foreach (var p in ports)
            {
                var userDefinedPin = PinPair.FirstOrDefault(i => i.name == p.Name);

                PortPosition pos; //默认为Left
                var order = -1;

                if (string.IsNullOrEmpty(userDefinedPin.name))
                {
                    pos = (PortPosition)r.Next(0, 4); // 用户没有定义则随机安排位置
                }
                else // 发现用户已定义
                {
                    pos = userDefinedPin.position;
                    order = userDefinedPin.order;
                }

                var isHorizontal = (pos == PortPosition.Left) || (pos == PortPosition.Right);
                var metalNum = m1direction == "horizontal" ?
                    (isHorizontal ? 3 : 4) :
                    (isHorizontal ? 4 : 3);

                var p2 = new Port
                {
                    MetalLayer = metalNum,
                    Name = p.Name,
                    Position = pos,
                    Width = p.Width,
                    Order = order
                };

                switch (pos)
                {
                    case PortPosition.Left:
                        LeftPorts.Add(p2);
                        if(order != -1) leftOrders.Add(order);
                        break;
                    case PortPosition.Top:
                        TopPorts.Add(p2);
                        if (order != -1) topOrders.Add(order);
                        break;
                    case PortPosition.Right:
                        RightPorts.Add(p2);
                        if (order != -1) rightOrders.Add(order);
                        break;
                    case PortPosition.Bottom:
                        BottomPorts.Add(p2);
                        if (order != -1) bottomOrders.Add(order);
                        break;
                }
            }

            // 第二次循环将order确定
            foreach (var p in LeftPorts)
            {
                if (p.Order == -1)
                {
                    for (int i = 1; ; i++)
                    {
                        if (!leftOrders.Contains(i))
                        {
                            p.Order = i;
                            leftOrders.Add(i);
                            break;
                        }
                    }
                }
            }

            LeftPorts.Sort((p1, p2) => p1.Order.CompareTo(p2.Order));

            foreach (var p in TopPorts)
            {
                if (p.Order == -1)
                {
                    for (int i = 1; ; i++)
                    {
                        if (!topOrders.Contains(i))
                        {
                            p.Order = i;
                            topOrders.Add(i);
                            break;
                        }
                    }
                }
            }
            TopPorts.Sort((p1, p2) => p1.Order.CompareTo(p2.Order));

            foreach (var p in RightPorts)
            {
                if (p.Order == -1)
                {
                    for (int i = 1; ; i++)
                    {
                        if (!rightOrders.Contains(i))
                        {
                            p.Order = i;
                            rightOrders.Add(i);
                            break;
                        }
                    }
                }
            }
            RightPorts.Sort((p1, p2) => p1.Order.CompareTo(p2.Order));

            foreach (var p in BottomPorts)
            {
                if (p.Order == -1)
                {
                    for (int i = 1; ; i++)
                    {
                        if (!bottomOrders.Contains(i))
                        {
                            p.Order = i;
                            bottomOrders.Add(i);
                            break;
                        }
                    }
                }
            }
            BottomPorts.Sort((p1, p2) => p1.Order.CompareTo(p2.Order));
        }
    }

    public enum PinPlaceMode
    {
        Uniform,
        ByOffset
    }

    public class FloorPlanSettings
    {
        public double LeftIO2Core { get; set; } = 4;
        public double RightIO2Core { get; set; } = 4;
        public double TopIO2Core { get; set; } = 4;
        public double BottomIO2Core { get; set; } = 4;

        public FloorPlanType FloorPlanType { get; set; } = FloorPlanType.AspectRatio;

        public double CoreUtilization { get; set; } = 0.7;
        public double AspectRatio { get; set; } = 1;

        public double CoreWidth { get; set; }
        public double CoreHeight { get; set; }
    }

    public enum FloorPlanType
    {
        AspectRatio,
        WidthHeight,
        WidthHeightAuto,
        Boundary
    }

    public class MacroPlaceSettings
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Orientation Orientation { get; set; }
        public double Width { get; set; } //无需用户指定,自动从LEF文件读取
        public double Height { get; set; } //无需用户指定,自动从LEF文件读取
        public List<string> PowerPins { get; set; } = new List<string>(); //无需用户指定,自动从LEF文件读取
        public List<string> GroundPins { get; set; } = new List<string>(); //无需用户指定,自动从LEF文件读取
        public double MarginLeft { get; set; } = 8;
        public double MarginTop { get; set; } = 8;
        public double MarginRight { get; set; } = 8;
        public double MarginBottom { get; set; } = 8;
        public bool CreateRing { get; set; } = false;
        public bool ReverseRoutingDirection { get; set; } = false;
    }

    public enum Orientation
    {
        N,E,S,W,FN,FE,FS,FW
    }

    internal class MacroInfo
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public List<string> PowerPins { get; set; }
        public List<string> GroundPins { get; set; }
    }

    public class APRRunnerSettings : HarborToolSettings
    {
        public bool UseICC { get; set; } = true;
        public bool UseICC2 { get; set; } = false;
        public bool UseInnovus { get; set; } = false;
        public string Top => ProjectInfo["Project"].Value<string>();
        public int MaxRoutingLayer { get; set; } = 4;
        public int MaxPreRouteLayer { get; set; } = 6;
        public double PowerWidth { get; set; } = 1;
        public double VerticalSpace { get; set; } = 0.3;
        public double VerticalOffset { get; set; } = 0.5;
        public double GroundWidth { get; set; } = 1;
        public double HorizontalSpace { get; set; } = 0.3;
        public double HorizontalOffset { get; set; } = 0.5;
        public double PowerStrapStart { get; set; } = 20;
        public double PowerStrapStep { get; set; } = 20;
        public bool CreatePowerStrap { get; set; } = false;
        public RouteSettings RouteSettings { get; set; } = new RouteSettings();
        public PlaceSettings PlaceSettings { get; set; } = new PlaceSettings();
        public FloorPlanSettings FloorPlanSettings { get; set; } = new FloorPlanSettings();
        public PinSettings PinSettings { get; set; } = new PinSettings();
        public DirectoryPath ProjectPath { get; set; }
        public DirectoryPath SynProjectPath { get; set; }
        public FilePathCollection Verilog { get; set; }
        public FilePathCollection AdditionalTimingDb { get; set; } = new FilePathCollection();
        public DirectoryPathCollection AdditionalRefLib { get; set; } = new DirectoryPathCollection();
        public bool AddPG { get; set; }
        public bool OpenGUI { get; set; }
        public bool FormalVerify { get; set; }

        private List<MacroInfo> MacroInfos { get; set; } = new List<MacroInfo>();

        public FilePath BuildScriptFile { get; set; }

        public ICCRunnerSettings GetIccRunnerSettings()
        {
            var settings = new ICCRunnerSettings
            {
                WorkingDirectory = WorkingDirectory,
                CommandFile = BuildScriptFile,
                CommandLogFile = CommandLogFile,
                GUI = OpenGUI
            };
            return settings;
        }

        internal override void GenerateTclScripts()
        {
            WorkingDirectory = ProjectPath.Combine("build");
            var (libInfo, pdk, io) = LibraryHelper.GetLibraryParams(ProjectInfo);

            if (FloorPlanSettings.FloorPlanType == FloorPlanType.WidthHeightAuto) //自动计算CoreHeight和CoreWidth
            {
                var area = GetTotalArea(SynProjectPath.Combine("rpt").CombineWithFilePath($"{Top}_area.rpt"));
                var area2 = area / FloorPlanSettings.CoreUtilization;
                var width = Math.Sqrt(area2 / FloorPlanSettings.AspectRatio);
                var height = Math.Sqrt(area2 * FloorPlanSettings.AspectRatio);

                if (area2 < 25) //Area过小可能导致Utilization超过100%
                {
                    height += 2;
                    width += 2;
                }

                FloorPlanSettings.CoreHeight = height;
                FloorPlanSettings.CoreWidth = width;
            }
            
            BuildTclModel model = new BuildTclModel
            {
                Library = ProjectInfo["Library"].Value<string>(),
                ScriptRootPath = WorkingDirectory.FullPath,
                TechFilePath = libInfo.techfile_full_name,
                RefLibPath = new List<string> { libInfo.ref_path },
                TopName = Top,
                SynNetlist = SynProjectPath.Combine("netlist").FullPath,
                Netlist = ProjectPath.Combine("netlist").FullPath,
                TLUPMax = libInfo.tluplus_worst_full_name,
                TLUPMin = libInfo.tluplus_best_full_name,
                Tech2itfMap = libInfo.tluplus_map_full_name,
                Power = libInfo.power_pin,
                Ground = libInfo.ground_pin,
                MaxRoutingLayer = MaxRoutingLayer,
                PowerWidth = PowerWidth,
                VerticalSpace = VerticalSpace,
                VerticalOffset = VerticalOffset,
                GroundWidth = GroundWidth,
                HorizontalSpace = HorizontalSpace,
                HorizontalOffset = HorizontalOffset,
                TapCell = libInfo.filltie_cell,
                Antenna = libInfo.antenna_full_name,
                AntennaCells = libInfo.antenna_cells,
                DelayCells = libInfo.delay_cells,
                Filler = libInfo.fill_cells,
                CapCells = libInfo.cap_cells,
                TieHighCell = libInfo.tie_high_cell,
                TieLowCell = libInfo.tie_low_cell,
                RptPath = ProjectPath.Combine("rpt").FullPath,
                GDSPath = ProjectPath.Combine("gds").FullPath,
                LibPath = libInfo.timing_db_path,
                LibName = libInfo.timing_db_name_abbr,
                LibFullName = libInfo.timing_db_name,
                GDSLayerMap = libInfo.gds_layer_map,
                Cores = Environment.ProcessorCount < 16 ? Environment.ProcessorCount : 16,
                M1RoutingDirection = libInfo.m1_routing_direction,
                MaxPreRouteLayer = MaxPreRouteLayer,
                PowerRailLayer = libInfo.power_rail_layer,
                PowerStrapStart = PowerStrapStart,
                PowerStrapStep = PowerStrapStep,
                CreatePowerStrap = CreatePowerStrap,

                FloorPlanSettings = FloorPlanSettings,

                MnTXT1 = pdk.GetLayerNumber("MnTXT1"),
                MnTXT2 = pdk.GetLayerNumber("MnTXT2"),
                MnTXT3 = pdk.GetLayerNumber("MnTXT3"),
                MnTXT4 = pdk.GetLayerNumber("MnTXT4"),
                MnTXT5 = pdk.GetLayerNumber("MnTXT5"),
                MnTXT6 = pdk.GetLayerNumber("MnTXT6"),
            };

            if (io != null && io.Count > 0)
            {
                model.IOTimingDbPaths = io.Select(i => System.IO.Path.Combine(i.timing_db_path, i.timing_db_name)).ToList();
                model.RefLibPath.AddRange(io.Select(i => i.icc_ref_path));
            }

            AdditionalRefLib = GetReferenceRefPath(AdditionalRefLib);
            model.RefLibPath.AddRange(AdditionalRefLib.Select(d => d.FullPath));
            AdditionalTimingDb = GetReferenceDb(AdditionalTimingDb);
            model.AdditionalTimingDbPaths = AdditionalTimingDb.Select(f => f.FullPath).ToList();

            InflateMacroPlaceSettings();
            model.MacroPlaceSettings = PlaceSettings.MacroPlaceSettings;

            model.StdCell = libInfo;

            IOHelper.CreateDirectory(ProjectPath);
            IOHelper.CreateDirectory(model.RptPath);
            IOHelper.CreateDirectory(model.GDSPath);
            IOHelper.CreateDirectory(model.Netlist);
            IOHelper.DeleteDirectory(WorkingDirectory);
            IOHelper.CreateDirectory(WorkingDirectory);


            var pinPadTclModel = new PinPadTclModel
            {
                ConstraintFile = PinSettings.ConstraintFile,
            };

            BuildScriptFile = "build.tcl";
            CommandLogFile = "build.log";

            var tran = "";
            if (UseICC)
            {
                var buildTcl = new ICCBuildTcl(model, pinPadTclModel);
                tran = buildTcl.TransformText();
            }
            else if (UseICC2)
            {
                
            }
            else if(UseInnovus)
            {
                
            }
            
            File.WriteAllText(WorkingDirectory.CombineWithFilePath(BuildScriptFile).FullPath, tran);

            if (pinPadTclModel.ConstraintFile == null)
            {
                PinSettings.InflatePorts(SynProjectPath, libInfo.m1_routing_direction, Top);
                pinPadTclModel.PinSpace = PinSettings.PinSpace;
                pinPadTclModel.PinPlaceMode = PinSettings.PinPlaceMode;
                pinPadTclModel.LeftPorts = PinSettings.LeftPorts;
                pinPadTclModel.TopPorts = PinSettings.TopPorts;
                pinPadTclModel.RightPorts = PinSettings.RightPorts;
                pinPadTclModel.BottomPorts = PinSettings.BottomPorts;

                var pinPadTcl = new PinPadTcl(pinPadTclModel);
                var tran2 = pinPadTcl.TransformText().Replace("\r\n", Environment.NewLine);
                File.WriteAllText(WorkingDirectory.CombineWithFilePath("pin_pad.tcl").FullPath, tran2);
            }
        }

        internal FilePathCollection GetReferenceDb(FilePathCollection additionalDb)
        {
            if (!ProjectInfo.ContainsKey("Reference"))
            {
                return additionalDb;
            }
            var refs = ProjectInfo["Reference"];
            foreach (var ref_ in refs)
            {
                var name = ref_["Name"].Value<string>();
                var path = ref_["Path"].Value<string>();

                var refProjcetJson = System.IO.Path.Combine(path, "project.json");
                var refProjectInfo = JObject.Parse(File.ReadAllText(refProjcetJson));
                var refProjectType = refProjectInfo["ProjectType"].Value<string>();
                switch (refProjectType)
                {
                    case "Analog":
                        break;
                    case "Memory": //当前只支持Memory
                        var refLibertyPath = System.IO.Path.Combine(path, "liberty");
                        var dbs = Directory.GetFiles(refLibertyPath, "*.db", SearchOption.TopDirectoryOnly).Select(p => new FileInfo(p));
                        var ttDb = dbs.FirstOrDefault(db => db.Name.Contains("tt"));
                        if (ttDb != null)
                        {
                            additionalDb.Add(ttDb.FullName);
                        }
                        break;
                    case "Digital":
                        break;
                    case "IP":
                        break;
                    default:
                        break;
                }

            }

            return additionalDb;
        }

        internal DirectoryPathCollection GetReferenceRefPath(DirectoryPathCollection additionalRefPath)
        {
            if (!ProjectInfo.ContainsKey("Reference"))
            {
                return additionalRefPath;
            }
            var refs = ProjectInfo["Reference"];
            foreach (var ref_ in refs)
            {
                var name = ref_["Name"].Value<string>();
                var path = ref_["Path"].Value<string>();

                var refProjcetJson = System.IO.Path.Combine(path, "project.json");
                var refProjectInfo = JObject.Parse(File.ReadAllText(refProjcetJson));
                var refProjectType = refProjectInfo["ProjectType"].Value<string>();
                switch (refProjectType)
                {
                    case "Analog":
                        break;
                    case "Memory": //当前只支持Memory
                        var refLibertyPath = new DirectoryInfo(System.IO.Path.Combine(path, "astro"));
                        var refLibPaths = refLibertyPath.GetDirectories();
                        var refLibPath = refLibPaths.FirstOrDefault(db => db.Name.ToLower().Contains(name.ToLower()));
                        if (refLibPath != null)
                        {
                            additionalRefPath.Add(refLibPath.FullName);
                        }
                        var refLefPath = new DirectoryInfo(System.IO.Path.Combine(path, "lef"));
                        var lefJsons = refLefPath.GetFiles("*.json").FirstOrDefault();
                        if (lefJsons != null)
                        {
                            var lefJ = JObject.Parse(File.ReadAllText(lefJsons.FullName));
                            var macroSize = (JArray)lefJ["macro_dict"][name]["info"]["SIZE"];

                            var pins = (JArray)lefJ["macro_dict"][name]["info"]["PIN"];
                            var powerPins = pins.Where(p => p["info"]["USE"].Value<string>() == "POWER")
                                .Select(p => p["name"].Value<string>()).ToList();
                            var groundPins = pins.Where(p => p["info"]["USE"].Value<string>() == "GROUND")
                                .Select(p => p["name"].Value<string>()).ToList();

                            MacroInfos.Add(new MacroInfo
                            {
                                Name = name,
                                Width = macroSize[0].Value<double>(),
                                Height = macroSize[1].Value<double>(),
                                PowerPins = powerPins,
                                GroundPins = groundPins
                            });

                        }
                        break;
                    case "Digital":
                        break;
                    case "IP":
                        break;
                    default:
                        break;
                }

            }

            return additionalRefPath;
        }

        void InflateMacroPlaceSettings()
        {
            for (int i = 0; i < PlaceSettings.MacroPlaceSettings.Count; i++)
            {
                var macroPlace = PlaceSettings.MacroPlaceSettings[i];
                var macro = MacroInfos.FirstOrDefault(m => m.Name == macroPlace.Type);
                if (macro != null)
                {
                    macroPlace.Width = macro.Width;
                    macroPlace.Height = macro.Height;
                    macroPlace.PowerPins.AddRange(macro.PowerPins);
                    macroPlace.GroundPins.AddRange(macro.GroundPins);
                }
            }
        }

        double GetTotalArea(FilePath areaReportFile)
        {
            var content = File.ReadAllLines(areaReportFile.FullPath);
            var l = content.FirstOrDefault(s => s.StartsWith("Total cell area:"));
            if (!string.IsNullOrEmpty(l))
            {
                l = l.Trim();
                var ls = l.Split(' ');
                double.TryParse(ls[^1], out var area);
                return area;
            }
            return 0;
        }
    }
}
