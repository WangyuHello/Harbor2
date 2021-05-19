using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Harbor.Core.Tool.APR.Model;

namespace Harbor.Core.Tool.APR.Tcl
{
    public partial class ICCBuildTcl
    {
        private BuildTclModel model;
        private PinPadTclModel pinModel;

        public ICCBuildTcl(BuildTclModel model, PinPadTclModel pinModel)
        {
            this.model = model;
            this.pinModel = pinModel;
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
