using System;
using System.Collections.Generic;
using System.Text;
using Harbor.Core.Util;

namespace Harbor.Core.Tool.APR.Model
{
    public class BuildTclModel
    {
        public string Library { get; set; }
        public string TechFilePath { get; set; }
        public List<string> RefLibPath { get; set; } = new List<string>();
        public List<string> AdditionalTimingDbPaths { get; set; } = new List<string>();
        public List<string> IOTimingDbPaths { get; set; } = new List<string>();
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
        public double PowerWidth { get; set; }
        public double VerticalSpace { get; set; }
        public double VerticalOffset { get; set; }
        public double GroundWidth { get; set; }
        public double HorizontalSpace { get; set; }
        public double HorizontalOffset { get; set; }
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
        public double PowerStrapStart { get; set; }
        public double PowerStrapStep { get; set; }
        public bool CreatePowerStrap { get; set; }

        public FloorPlanSettings FloorPlanSettings { get; set; }

        public int MnTXT1 { get; set; }
        public int MnTXT2 { get; set; }
        public int MnTXT3 { get; set; }
        public int MnTXT4 { get; set; }
        public int MnTXT5 { get; set; }
        public int MnTXT6 { get; set; }

        public List<MacroPlaceSettings> MacroPlaceSettings { get; set; } = new List<MacroPlaceSettings>();

        public LibraryHelper.LibraryStdCell StdCell { get; set; }
    }
}
