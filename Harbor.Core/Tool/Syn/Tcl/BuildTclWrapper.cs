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
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                tran = tran.Replace("\r", "");
            }
            File.WriteAllText(file, tran, new UTF8Encoding(false));
        }
    }
}
