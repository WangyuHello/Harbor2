using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Harbor.Core.Tool.APR.Tcl
{
    public partial class PinPadTcl
    {
        private readonly PinSettings model;

        public PinPadTcl(PinSettings model)
        {
            this.model = model;
        }

        public void WriteToFile(string file)
        {
            var tran = TransformText();
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                tran = tran.Replace("\r", "");
            }
            File.WriteAllText(file, tran, new UTF8Encoding(false));
        }
    }
}
