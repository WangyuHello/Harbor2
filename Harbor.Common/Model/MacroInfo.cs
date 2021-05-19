using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Common.Model
{
    public class MacroInfo
    {
        public string Name { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public List<string> PowerPins { get; set; }
        public List<string> GroundPins { get; set; }
    }
}
