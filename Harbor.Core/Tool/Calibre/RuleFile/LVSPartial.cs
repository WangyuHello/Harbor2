using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
// ReSharper disable InconsistentNaming

namespace Harbor.Core.Tool.Calibre.RuleFile
{
    public partial class LVS
    {
        public string GDSFullPath { get; set; }
        public string PrimaryCell { get; set; }
        public string NetlistFullPath { get; set; }
        public string LVSRuleFullPath { get; set; }

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
