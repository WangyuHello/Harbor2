using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.IO;
using Harbor.Common.Project;
using Harbor.Core.Tool.Calibre.RuleFile;
// ReSharper disable InconsistentNaming

namespace Harbor.Core.Tool.Calibre
{
    class CalibreLVSSettings : HarborToolSettings
    {
        public FilePath GDSFile { get; set; }
        public string PrimaryCell { get; set; }
        public FilePath NetlistFile { get; set; }

        internal override void GenerateRunScripts()
        {
            var library = AllLibrary.GetLibrary(ProjectInfo);
            var lvsRule = library.Pdk.lvs_full_name;
            var emptyCircuit = library.Pdk.empty_circuit_full_name;

            var lvs = new LVS
            {
                GDSFullPath = GDSFile.FullPath,
                PrimaryCell = PrimaryCell,
                NetlistFullPath = "run.sp",
                LVSRuleFullPath = lvsRule
            };
            lvs.WriteToFile(WorkingDirectory.CombineWithFilePath("run.lvs").FullPath);

            WriteNetlistFile(NetlistFile, emptyCircuit, WorkingDirectory.CombineWithFilePath("run.sp"));
        }

        private void WriteNetlistFile(FilePath netlistFile, string emptyCircuit, FilePath path)
        {
            StringBuilder sb = new();
            sb.AppendLine($".INCLUDE \"{emptyCircuit}\"");
            sb.AppendLine($".INCLUDE \"{netlistFile.FullPath}\"");
            var content = sb.ToString();
            File.WriteAllText(path.FullPath, content, new UTF8Encoding(false));
        }

        public CalibreLVSSettings(ICakeContext context) : base(context)
        {

        }

        public CalibreRunnerSettings ToCalibreRunnerSettings()
        {
            var settings = new CalibreRunnerSettings()
            {
                DRC = true,
                WorkingDirectory = WorkingDirectory
            };
            return settings;
        }
    }
}
