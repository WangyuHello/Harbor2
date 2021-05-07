using Harbor.Core.Tool.Syn.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Harbor.Core.Tool.Syn.Tcl
{
    public partial class BuildTcl
    {
        private readonly BuildTclModel model;
        public BuildTcl(BuildTclModel model)
        {
            this.model = model;
        }

        public void WriteToFile(string file)
        {
            var tran = TransformText();
            File.WriteAllText(file, tran);
        }
    }
}
