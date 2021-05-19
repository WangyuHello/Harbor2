using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Harbor.Core.Tool.APR.Model;

namespace Harbor.Core.Tool.APR.Tcl
{
    public partial class PinPadTcl
    {
        private PinPadTclModel model;

        public PinPadTcl(PinPadTclModel model)
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
