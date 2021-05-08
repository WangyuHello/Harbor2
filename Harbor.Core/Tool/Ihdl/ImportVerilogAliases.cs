using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Harbor.Core.Project;

namespace Harbor.Core.Tool.Ihdl
{
    public static class ImportVerilogAliases
    {
        [CakeMethodAlias]
        public static void ImportVerilog(this ICakeContext context, string vName, string libName, string topCellName)
        {
            var projectInfo = ProjectInfo.ReadFromContext(context);
            var library = AllLibrary.GetLibrary(projectInfo);

            var refLibList = new List<string>
            {
                library.Pdk.pdk_name
            };
            refLibList.AddRange(library.StdCell.Select(s => s.cdk_name));

            if (projectInfo.Reference != null)
            {
                refLibList.AddRange(projectInfo.Reference.Select(r => r.Name));
            }

            var template = new IhdlFile.IhdlFile
            {
                DestSchLib = libName,
                RefLibList = refLibList,
                TopCellName = topCellName
            };
            var templateText = template.TransformText();
            File.WriteAllText(context.MakeAbsolute(new FilePath($"./.harbor/{topCellName}.ihdlFile")).FullPath, templateText.Replace("\r", ""), encoding: Encoding.UTF8);

            context.Ihdl($"./.harbor/{topCellName}.ihdlFile", library.StdCell.First().cdk_name, vName);
        }

        [CakeMethodAlias]
        public static void ImportVerilogFunctional(this ICakeContext context, string vName, string libName,
            string topCellName)
        {
            var template = new IhdlFile.IhdlFileFunctional
            {
                DestSchLib = libName,
                TopCellName = topCellName
            };
            var templateText = template.TransformText();
            File.WriteAllText(context.MakeAbsolute(new FilePath($"./.harbor/{topCellName}.ihdlFile")).FullPath, templateText.Replace("\r", ""), encoding: Encoding.UTF8);

            context.Ihdl($"./.harbor/{topCellName}.ihdlFile", vName);
        }
    }
}
