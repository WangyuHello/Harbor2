// ReSharper disable InconsistentNaming
namespace Harbor.Common.Model
{
    public class Port
    {
        public int MetalLayer { get; set; }
        public string Name { get; set; }
        public Width Width { get; set; }
        public PinPosition Position { get; set; }
        public int Order { get; set; }
    }

    public class Width
    {
        public int msb { get; set; }
        public int lsb { get; set; }
    }

    public enum PinPosition
    {
        Left,
        Top,
        Right,
        Bottom
    }
}
