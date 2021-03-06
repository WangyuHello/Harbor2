using System;
using System.Collections.Generic;
using System.Text;
using Harbor.Common.Project;

namespace Harbor.Core.Tool.APR.Model
{
    public class BuildTclModel : HarborTextModel
    {
        public bool FloorPlanOnly { get; set; }
        public string Library { get; set; }
        public string TechFilePath { get; set; }
        public List<string> RefLibPath { get; set; } = new();
        public List<string> AdditionalTimingDbPaths { get; set; } = new();
        public List<string> IOTimingDbPaths { get; set; } = new();
        public string LibPath { get; set; }
        public string LibName { get; set; }
        public string LibFullName { get; set; }
        public int Cores { get; set; }
        public string TopName { get; set; }
        public string Netlist { get; set; }
        public string SynNetlist { get; set; }
        public string TLUPMax { get; set; }
        public string TLUPMin { get; set; }
        public string Tech2itfMap { get; set; }
        public string ScriptRootPath { get; set; }
        public string Power { get; set; }
        public string Ground { get; set; }
        public int MaxRoutingLayer { get; set; }
        public int MaxPreRouteLayer { get; set; }

        public string TapCell { get; set; }
        public string Antenna { get; set; }
        public string[] AntennaCells { get; set; }
        public string[] DelayCells { get; set; }
        public string[] Filler { get; set; }
        public string[] CapCells { get; set; }
        public string TieHighCell { get; set; }
        public string TieLowCell { get; set; }
        public string RptPath { get; set; }
        public string GDSPath { get; set; }
        public string GDSLayerMap { get; set; }
        public string M1RoutingDirection { get; set; }
        public int PowerRailLayer { get; set; }

        public string PinConstrainFilePath { get; set; }
        public FloorPlanSettings FloorPlanSettings { get; set; }

        public int MnTXT1 { get; set; }
        public int MnTXT2 { get; set; }
        public int MnTXT3 { get; set; }
        public int MnTXT4 { get; set; }
        public int MnTXT5 { get; set; }
        public int MnTXT6 { get; set; }

        public List<MacroPlaceSettings> MacroPlaceSettings { get; set; } = new();

        public Library.LibraryStdCell StdCell { get; set; }
    }
}
