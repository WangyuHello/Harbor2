using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harbor.Commands.Project;

namespace Harbor.Commands.Template
{
    public partial class CdsLib
    {
        public Library.LibraryPdk Pdk { get; set; }
        public List<Library.LibraryStdCell> StdCell { get; set; }
        public List<Library.LibraryIo> Io { get; set; }
    }
}
