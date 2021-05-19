using System.Collections.Generic;
using Harbor.Common.Model;

namespace Harbor.Core.Tool.APR.Model
{
    public class PinPadTclModel : HarborTextModel
    {
        public List<Port> LeftPorts { get; set; }
        public List<Port> TopPorts { get; set; }
        public List<Port> RightPorts { get; set; }
        public List<Port> BottomPorts { get; set; }
        public decimal PinSpace { get; set; }
        public PinPlaceMode PinPlaceMode { get; set; }
    }
}
