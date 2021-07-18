using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.IO;
using Harbor.Common.Project;
using Harbor.Core.Util.Template;
using Path = System.IO.Path;

namespace Harbor.Core.Util
{
    public static class CdsUtil
    {
        public static async Task CreateCdsLibAsync(string dir, Library.LibraryPdk pdk, List<Library.LibraryStdCell> stdCell, List<Library.LibraryIo> io)
        {
            var cdsLib = new CdsLib
            {
                Pdk = pdk,
                StdCell = stdCell,
                Io = io
            };
            var text = cdsLib.TransformText();
            await File.WriteAllTextAsync(Path.Combine(dir, "cds.lib"), text.Replace("\r",""), new UTF8Encoding(false));

            var cdsInit = new CdsInit
            {
                Pdk = pdk
            };
            text = cdsInit.TransformText();
            await File.WriteAllTextAsync(Path.Combine(dir, ".cdsinit"), text.Replace("\r", ""), new UTF8Encoding(false));

            var cdsEnv = new CdsEnv();
            text = cdsEnv.TransformText();
            await File.WriteAllTextAsync(Path.Combine(dir, ".cdsenv"), text.Replace("\r", ""), new UTF8Encoding(false));

            var simRc = new SimRc();
            text = simRc.TransformText();
            await File.WriteAllTextAsync(Path.Combine(dir, ".simrc"), text.Replace("\r", ""), new UTF8Encoding(false));
        }

        private static string GetReferencePath(DirectoryPath dir, DirectoryPath referncePath)
        {
            //referncePath是相对路径
            var refProjectInfo = ProjectInfo.ReadFromDirectory(dir.Combine(referncePath));
            return refProjectInfo.Type switch
            {
                ProjectType.Digital => referncePath.Combine($"./Cadence/{refProjectInfo.Project}/{refProjectInfo.Project}").FullPath,
                ProjectType.Analog => throw new NotImplementedException(),
                ProjectType.Memory => referncePath.Combine($"./Cadence/{refProjectInfo.Project}/{refProjectInfo.Project}").FullPath,
                ProjectType.Ip => referncePath.Combine($"./Cadence/{refProjectInfo.Project}/{refProjectInfo.Project}").FullPath,
                _ => throw new NotImplementedException()
            };
        }

        public static async Task RefreshCdsLibAsync(ProjectInfo projectInfo)
        {
            var library = AllLibrary.GetLibrary(projectInfo);

            var references = projectInfo.Reference;//相对于project.json的相对路径
            var pdk = library.Pdk;
            var stdCells = library.StdCell;
            var ios = library.Io;

            var definePairs = new Dictionary<string, (string path, bool found)>();
            references?.ForEach(r => definePairs.Add(r.Name, (GetReferencePath(projectInfo.Directory, r.Path), false)));
            definePairs.Add(pdk.pdk_name, (pdk.pdk_path, false));
            stdCells?.ForEach(s => definePairs.Add(s.cdk_name, (s.cdk_path, false)));
            ios?.ForEach(s => definePairs.Add(s.cdk_name, (s.cdk_path, false)));

            var cdsLibContent = await File.ReadAllLinesAsync(projectInfo.Directory.CombineWithFilePath("cds.lib").FullPath);

            for (int i = 0; i < cdsLibContent.Length; i++)
            {
                var line = cdsLibContent[i];
                var line2 = line.Trim();
                cdsLibContent[i] = line2;
                if (!line2.StartsWith("DEFINE")) continue;
                var segs = line2.Split(' ');
                var define1 = segs[1];
                var define2 = segs[2];

                if (!definePairs.ContainsKey(define1)) continue;
                var pair = definePairs[define1];
                pair.found = true;
                definePairs[define1] = pair;

                if (define2 != pair.path)
                {
                    cdsLibContent[i] = $"DEFINE {define1} {pair.path}";
                }
            }

            var newCdsLibContent = new List<string>(cdsLibContent);
            
            foreach (var definePair in definePairs)
            {
                if (definePair.Value.found == false)
                {
                    newCdsLibContent.Add($"DEFINE {definePair.Key} {definePair.Value.path}");
                }
            }

            await File.WriteAllLinesAsync(projectInfo.Directory.CombineWithFilePath("cds.lib").FullPath, newCdsLibContent, new UTF8Encoding(false));
        }
    }
}
