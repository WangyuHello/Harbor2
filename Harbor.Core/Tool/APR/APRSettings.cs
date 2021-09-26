using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Common.IO;
using Harbor.Common.Model;
using Harbor.Common.Project;
using Harbor.Common.Util;
using Harbor.Core.Tool.APR.Model;
using Harbor.Core.Tool.APR.Tcl;
using Harbor.Core.Tool.ICC;
using Harbor.Core.Util;
using Harbor.Python.Tool;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace Harbor.Core.Tool.APR
{
    public class RouteSettings
    {
        
    }

    public class PlaceSettings
    {
        public List<MacroPlaceSettings> MacroPlaceSettings { get; set; } = new();
    }

    public interface IPinSettings
    {
        public PinPosition Position { get; set; }
        public decimal Offset { get; set; }
        public string Layer { get; set; }
        public bool ReverseBusOrder { get; set; }
    }

    public class SinglePinSettings : IPinSettings
    {
        public PinPosition Position { get; set; }
        public decimal Offset { get; set; }
        public string Layer { get; set; }
        public bool ReverseBusOrder { get; set; }

        public string Name { get; set; }
    }

    public class PinGroupSettings : IPinSettings
    {
        public PinPosition Position { get; set; }
        public decimal Offset { get; set; }
        public string Layer { get; set; }
        public bool ReverseBusOrder { get; set; }

        public List<SinglePinSettings> Pins { get; set; }
        public decimal Space { get; set; }
    }

    public class PinSettings
    {
        public List<IPinSettings> LeftPins { get; set; } = new();
        public List<IPinSettings> TopPins { get; set; } = new();
        public List<IPinSettings> RightPins { get; set; } = new();
        public List<IPinSettings> BottomPins { get; set; } = new();

        public PinPlaceMode PinPlaceMode { get; set; } = PinPlaceMode.Uniform;
        public decimal Space { get; set; }
        public string VercitalLayer { get; set; }
        public string HorizontalLayer { get; set; }

        public Dictionary<string, VerilogPortDefinition> VerilogPorts { get; set; }

        /// <summary>
        /// 如果指定了pin约束脚本,则直接引用,不自动生成约束
        /// </summary>
        public FilePath ConstraintFile { get; set; }

        public void SetDefaultLayer(string m1_routing_direction)
        {
            var defaultVerticalLayer = m1_routing_direction switch
            {
                "horizontal" => "M4",
                "vertical" => "M3",
                _ => throw new NotImplementedException("不支持的M1走线方向")
            };

            var defaulthorizontalLayer = m1_routing_direction switch
            {
                "horizontal" => "M3",
                "vertical" => "M4",
                _ => throw new NotImplementedException("不支持的M1走线方向")
            };

            if (string.IsNullOrEmpty(VercitalLayer))
            {
                VercitalLayer = defaultVerticalLayer;
            }

            if (string.IsNullOrEmpty(HorizontalLayer))
            {
                HorizontalLayer = defaulthorizontalLayer;
            }
        }

        public void ReadVerilogPorts(DirectoryPath synProjectPath, string top)
        {
            var ports = GetPorts.Run2(
                synProjectPath.Combine("netlist").CombineWithFilePath($"{top}.v").FullPath, top, synProjectPath.Combine("netlist").FullPath);
            VerilogPorts = ports.ToDictionary(v => v.Name);
        }
    }

    public class PowerStrapSettings
    {
        public string Layer { get; set; }
        public List<string> Nets { get; set; } = new();
        public double Start { get; set; } = 20;
        public double Step { get; set; } = 20;
        public double? Stop { get; set; }
        public double? Space { get; set; }
        public double Width { get; set; } = 2;
        public bool Orientation { get; set; } = false; // true => horizontal false => vertical
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

        public double? CoreWidth { get; set; }
        public double? CoreHeight { get; set; }

        public double? Width { get; set; }
        public double? Height { get; set; }

        public double VerticalWidth { get; set; } = 1;
        public double VerticalSpace { get; set; } = 0.3;
        public double VerticalOffset { get; set; } = 0.5;
        public double HorizontalWidth { get; set; } = 1;
        public double HorizontalSpace { get; set; } = 0.3;
        public double HorizontalOffset { get; set; } = 0.5;

        public PowerSettings PowerSettings { get; set; } = new();

        public List<string> PowerRingNets { get; set; } = new();

        public List<PowerStrapSettings> PowerStraps { get; set; } = new();
    }

    public class MacroPlaceSettings
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Orientation Orientation { get; set; }
        public double? Width { get; set; } //无需用户指定,自动从LEF文件读取
        public double? Height { get; set; } //无需用户指定,自动从LEF文件读取
        public List<string> PowerPins { get; set; } = new(); //无需用户指定,自动从LEF文件读取
        public List<string> GroundPins { get; set; } = new(); //无需用户指定,自动从LEF文件读取
        public double MarginLeft { get; set; } = 8;
        public double MarginTop { get; set; } = 8;
        public double MarginRight { get; set; } = 8;
        public double MarginBottom { get; set; } = 8;
        public bool CreateRing { get; set; } = false;
        public bool ReverseRoutingDirection { get; set; } = false;

        public Dictionary<string,string> PowerConnections { get; set; }
    }

    public class PowerSettings
    {
        public string PrimaryPower { get; set; } = "VDD";
        public string PrimaryGround { get; set; } = "VSS";
        public List<string> AdditionalPower { get; set; } = new();
    }

    public class APRRunnerSettings : HarborToolSettings
    {
        public bool UseICC { get; set; } = true;
        public bool UseICC2 { get; set; } = false;
        public bool UseInnovus { get; set; } = false;
        public string Top => ProjectInfo.Project;
        public int MaxRoutingLayer { get; set; } = 4;
        public int MaxPreRouteLayer { get; set; } = 6;

        public bool FloorPlanOnly { get; set; } = false;

        public RouteSettings RouteSettings { get; set; } = new();
        public PlaceSettings PlaceSettings { get; set; } = new();
        public FloorPlanSettings FloorPlanSettings { get; set; } = new();
        public PinSettings PinSettings { get; set; } = new();
        public DirectoryPath ProjectPath { get; set; }
        public DirectoryPath SynProjectPath { get; set; }
        public FilePathCollection Verilog { get; set; }
        public FilePathCollection AdditionalTimingDb { get; set; } = new();
        public DirectoryPathCollection AdditionalRefLib { get; set; } = new();
        public bool AddPG { get; set; }
        public bool OpenGUI { get; set; }
        public bool FormalVerify { get; set; }
        public bool LVS { get; set; }

        private List<MacroInfo> MacroInfos { get; } = new();

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

        internal override void GenerateRunScripts()
        {
            WorkingDirectory = ProjectPath.Combine("build");
            var library = AllLibrary.GetLibrary(ProjectInfo);

            if (FloorPlanSettings.FloorPlanType == FloorPlanType.WidthHeightAuto) //自动计算CoreHeight和CoreWidth
            {
                var area = GetTotalArea(SynProjectPath.Combine("rpt").CombineWithFilePath($"{Top}_area.rpt"));
                var area2 = area / FloorPlanSettings.CoreUtilization;
                var width = Math.Sqrt(area2 / FloorPlanSettings.AspectRatio);
                var height = Math.Sqrt(area2 * FloorPlanSettings.AspectRatio);
                FloorPlanSettings.CoreHeight = height;
                FloorPlanSettings.CoreWidth = width;
            }

            if (FloorPlanSettings.CoreWidth == null || FloorPlanSettings.CoreHeight == null)
            {
                FloorPlanSettings.CoreWidth = FloorPlanSettings.Width - FloorPlanSettings.LeftIO2Core -
                                              FloorPlanSettings.RightIO2Core;
                FloorPlanSettings.CoreHeight = FloorPlanSettings.Height - FloorPlanSettings.TopIO2Core -
                                              FloorPlanSettings.BottomIO2Core;
            }
            
            var model = new BuildTclModel
            {
                FloorPlanOnly = FloorPlanOnly,
                Library = ProjectInfo.Library,
                ScriptRootPath = WorkingDirectory.FullPath,
                TechFilePath = library.PrimaryStdCell.techfile_full_name,
                RefLibPath = new List<string> { library.PrimaryStdCell.ref_path },
                TopName = Top,
                SynNetlist = SynProjectPath.Combine("netlist").FullPath,
                Netlist = ProjectPath.Combine("netlist").FullPath,
                TLUPMax = library.PrimaryStdCell.tluplus_worst_full_name,
                TLUPMin = library.PrimaryStdCell.tluplus_best_full_name,
                Tech2itfMap = library.PrimaryStdCell.tluplus_map_full_name,
                Power = library.PrimaryStdCell.power_pin,
                Ground = library.PrimaryStdCell.ground_pin,
                MaxRoutingLayer = MaxRoutingLayer,
                TapCell = library.PrimaryStdCell.filltie_cell,
                Antenna = library.PrimaryStdCell.antenna_full_name,
                AntennaCells = library.PrimaryStdCell.antenna_cells,
                DelayCells = library.PrimaryStdCell.delay_cells,
                Filler = library.PrimaryStdCell.fill_cells,
                CapCells = library.PrimaryStdCell.cap_cells,
                TieHighCell = library.PrimaryStdCell.tie_high_cell,
                TieLowCell = library.PrimaryStdCell.tie_low_cell,
                RptPath = ProjectPath.Combine("rpt").FullPath,
                GDSPath = ProjectPath.Combine("gds").FullPath,
                LibPath = library.PrimaryStdCell.timing_db_path,
                LibName = library.PrimaryStdCell.timing_db_name_abbr,
                LibFullName = library.PrimaryStdCell.timing_db_name,
                GDSLayerMap = library.PrimaryStdCell.gds_layer_map,
                Cores = System.Environment.ProcessorCount < 16 ? System.Environment.ProcessorCount : 16,
                M1RoutingDirection = library.PrimaryStdCell.m1_routing_direction,
                MaxPreRouteLayer = MaxPreRouteLayer,
                PowerRailLayer = library.PrimaryStdCell.power_rail_layer,

                FloorPlanSettings = FloorPlanSettings,

                MnTXT1 = library.Pdk.GetLayerNumber("MnTXT1"),
                MnTXT2 = library.Pdk.GetLayerNumber("MnTXT2"),
                MnTXT3 = library.Pdk.GetLayerNumber("MnTXT3"),
                MnTXT4 = library.Pdk.GetLayerNumber("MnTXT4"),
                MnTXT5 = library.Pdk.GetLayerNumber("MnTXT5"),
                MnTXT6 = library.Pdk.GetLayerNumber("MnTXT6"),
            };

            if (library.Io is {Count: > 0})
            {
                model.IOTimingDbPaths = library.Io.Select(i => System.IO.Path.Combine(i.timing_db_path, i.timing_db_name)).ToList();
                model.RefLibPath.AddRange(library.Io.Select(i => i.icc_ref_path));
            }

            AdditionalRefLib = ProjectUtil.GetReferenceRefPath(AdditionalRefLib, MacroInfos, ProjectInfo);
            model.RefLibPath.AddRange(AdditionalRefLib.Select(d => d.FullPath));
            AdditionalTimingDb = ProjectUtil.GetReferenceDb(AdditionalTimingDb, ProjectInfo);
            model.AdditionalTimingDbPaths = AdditionalTimingDb.Select(f => f.FullPath).ToList();

            InflateMacroPlaceSettings();
            model.MacroPlaceSettings = PlaceSettings.MacroPlaceSettings;

            model.StdCell = library.PrimaryStdCell;

            IOHelper.CreateDirectory(ProjectPath);
            IOHelper.CreateDirectory(model.RptPath);
            IOHelper.CreateDirectory(model.GDSPath);
            IOHelper.CreateDirectory(model.Netlist);
            IOHelper.DeleteDirectory(WorkingDirectory);
            IOHelper.CreateDirectory(WorkingDirectory);

            if (PinSettings.ConstraintFile == null)
            {
                PinSettings.SetDefaultLayer(library.PrimaryStdCell.m1_routing_direction);
                PinSettings.ReadVerilogPorts(SynProjectPath, Top);
                var pinPadTcl = new PinPadTcl(PinSettings);
                pinPadTcl.WriteToFile(WorkingDirectory.CombineWithFilePath("pin_pad.tcl").FullPath);
            }
            else
            {
                if (!Context.FileExists(PinSettings.ConstraintFile))
                {
                    throw new ArgumentException("ConstraintFile不存在");
                }

                var ext = PinSettings.ConstraintFile.GetExtension();
                switch (ext)
                {
                    case ".tcl": //指定了tcl文件
                        model.PinConstrainFilePath = Context.MakeAbsolute(PinSettings.ConstraintFile).FullPath;
                        break;
                    case ".xlsx":
                    case ".xls":
                    case ".csv": //指定了表格文件
                        GeneratePinConstraintFromExcel(PinSettings.ConstraintFile);
                        break;
                }
                
            }

            BuildScriptFile = "build.tcl";
            CommandLogFile = "build.log";

            if (UseICC)
            {
                var buildTcl = new ICCBuildTcl(model);
                buildTcl.WriteToFile(WorkingDirectory.CombineWithFilePath(BuildScriptFile).FullPath);
            }
            else if (UseICC2)
            {
                
            }
            else if(UseInnovus)
            {
                
            }

        }

        private void GeneratePinConstraintFromExcel(FilePath excelFile)
        {
            var data = ExcelHelper.ExcelToDataTable(excelFile.FullPath, null, true);
        }

        private void InflateMacroPlaceSettings()
        {
            foreach (var macroPlace in PlaceSettings.MacroPlaceSettings)
            {
                var place = macroPlace;
                var macro = MacroInfos.FirstOrDefault(m => m.Name == place.Type);
                if (macro == null) continue;
                macroPlace.Width = macro.Width;
                macroPlace.Height = macro.Height;
                macroPlace.PowerPins.AddRange(macro.PowerPins);
                macroPlace.GroundPins.AddRange(macro.GroundPins);
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
