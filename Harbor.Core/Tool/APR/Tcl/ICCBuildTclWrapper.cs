using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Harbor.Core.Tool.APR.Model;

namespace Harbor.Core.Tool.APR.Tcl
{
    public partial class ICCBuildTcl
    {
        private readonly BuildTclModel model;

        public ICCBuildTcl(BuildTclModel model)
        {
            this.model = model;
        }

        public void WriteToFile(string file)
        {
            var tran = TransformText();
            if (System.Environment.OSVersion.Platform == PlatformID.Unix)
            {
                tran = tran.Replace("\r", "");
            }
            File.WriteAllText(file, tran, new UTF8Encoding(false));
        }
    }
}
