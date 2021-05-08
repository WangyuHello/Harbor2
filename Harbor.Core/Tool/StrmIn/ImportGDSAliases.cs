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
using Cake.FileHelpers;
using Harbor.Core.Project;

namespace Harbor.Core.Tool.StrmIn
{
    public static class ImportGDSAliases
    {
        [CakeMethodAlias]
        public static void ImportGDS(this ICakeContext context, DirectoryPath directory, FilePath gds, string lib, string topCellName)
        {
            var projectInfo = ProjectInfo.ReadFromDirectory(directory.FullPath);
            var library = AllLibrary.GetLibrary(projectInfo);

            var createLib = !context.DirectoryExists(new DirectoryPath(lib));

            var refLibList = new List<string>
            {
                library.Pdk.pdk_name
            };
            refLibList.AddRange(library.StdCell.Select(s => s.cdk_name));

            if (projectInfo.Reference != null)
            {
                refLibList.AddRange(projectInfo.Reference.Select(r => r.Name));
            }
            context.FileWriteLines(directory.CombineWithFilePath("./.harbor/reflib.list"), refLibList.ToArray());

            var settings = new StrmInRunnerSettings
            {
                Library = lib,
                StrmFile = gds,
                LogFile = $"./harbor/{topCellName}.stream.log",
                View = "layout",
                Case = StrmInCase.preserve,
                ReplaceBusBitChar = true,
                LayerMapFile = library.Pdk.layer_map_in_full_name,
                RefLibList = "./.harbor/reflib.list",
                WorkingDirectory = directory
            };

            if (createLib)
            {
                settings.AttachTechFileOfLib = library.Pdk.pdk_name;
            }

            context.StrmIn(settings);
        }
    }
}
