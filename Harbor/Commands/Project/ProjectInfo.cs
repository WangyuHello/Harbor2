using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Commands.Project
{
    public sealed class ProjectInfo
    {
        public string Library { get; set; }
        public List<string> StdCell { get; set; }
        public List<string> Io { get; set; }
        public ProjectType Type { get; set; }
        public string Project { get; set; }
    }
}
