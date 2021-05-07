using System;
using System.Collections.Generic;
using System.Text;
using Harbor.Core.Tool.LC.Model;

namespace Harbor.Core.Tool.LC.Tcl
{
    public partial class BuildTcl
    {
        private readonly BuildTclModel model;
        public BuildTcl(BuildTclModel model)
        {
            this.model = model;
        }
    }
}
