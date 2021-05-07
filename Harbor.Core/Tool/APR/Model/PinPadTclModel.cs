using Harbor.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core.IO;

namespace Harbor.Core.Tool.APR.Model
{
    public class PinPadTclModel
    {
        public List<Port> LeftPorts { get; set; }
        public List<Port> TopPorts { get; set; }
        public List<Port> RightPorts { get; set; }
        public List<Port> BottomPorts { get; set; }
        public decimal PinSpace { get; set; }
        public FilePath ConstraintFile { get; set; }
        public PinPlaceMode PinPlaceMode { get; set; }
    }

    public class Port
    {
        public int MetalLayer { get; set; }
        public string Name { get; set; }
        public Width Width { get; set; }
        public PortPosition Position { get; set; }
        public int Order { get; set; }
    }

    public class Width
    {
        public int msb { get; set; }
        public int lsb { get; set; }
    }

    public enum PortPosition
    {
        Left,
        Top,
        Right,
        Bottom
    }
}
