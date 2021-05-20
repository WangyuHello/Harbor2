using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Core.IO;
using Harbor.Common.Model;
using Harbor.Common.Util;

namespace Harbor.Common.Project
{
    public static class ProjectUtil
    {
        public static FilePathCollection GetReferenceDb(FilePathCollection additionalDb, ProjectInfo projectInfo)
        {
            if (projectInfo.Reference == null)
            {
                return additionalDb;
            }
            var refs = projectInfo.Reference;
            foreach (var @ref in refs)
            {
                var path = @ref.Path;

                var refProjectInfo = ProjectInfo.ReadFromDirectory(path);
                var refProjectType = refProjectInfo.Type;
                switch (refProjectType)
                {
                    case ProjectType.Analog:
                        break;
                    case ProjectType.Memory: //当前只支持Memory
                        var refLibertyPath = System.IO.Path.Combine(path, "liberty");
                        var dbs = Directory.GetFiles(refLibertyPath, "*.db", SearchOption.TopDirectoryOnly)
                            .Select(p => new FileInfo(p)).ToArray();
                        var ssDb = dbs.FirstOrDefault(db => db.Name.Contains("ss"));
                        var ttDb = dbs.FirstOrDefault(db => db.Name.Contains("tt"));
                        if (ssDb is {Length: > 0})
                        {
                            additionalDb.Add(ssDb.FullName);
                        }
                        else if (ttDb is {Length: > 0})
                        {
                            additionalDb.Add(ttDb.FullName);
                        }

                        break;
                    case ProjectType.Digital:
                        break;
                    case ProjectType.Ip:
                        break;
                }
            }

            return additionalDb;
        }

        public static DirectoryPathCollection GetReferenceRefPath(DirectoryPathCollection additionalRefPath, List<MacroInfo> macroInfos, ProjectInfo projectInfo)
        {
            if (projectInfo.Reference == null)
            {
                return additionalRefPath;
            }
            var refs = projectInfo.Reference;
            foreach (var @ref in refs)
            {
                var name = @ref.Name;
                var path = @ref.Path;

                var refProjectInfo = ProjectInfo.ReadFromDirectory(path);
                var refProjectType = refProjectInfo.Type;
                switch (refProjectType)
                {
                    case ProjectType.Analog:
                        break;
                    case ProjectType.Memory: //当前只支持Memory
                        var refLibertyPath = new DirectoryInfo(System.IO.Path.Combine(path, "astro"));
                        var refLibPaths = refLibertyPath.GetDirectories();
                        var refLibPath = refLibPaths.FirstOrDefault(db => db.Name.ToLower().Contains(name.ToLower()));
                        if (refLibPath != null)
                        {
                            additionalRefPath.Add(refLibPath.FullName);
                        }
                        var refLefPath = new DirectoryInfo(System.IO.Path.Combine(path, "lef"));
                        var lefFile = refLefPath.GetFiles("*.lef").FirstOrDefault();
                        if (lefFile != null)
                        {
                            var lef = LefObject.Parse(lefFile.FullName);
                            var macro = lef.Macros[name];
                            var macroSize = macro.Size;

                            var pins = macro.Pins;
                            var powerPins = pins.Where(p => p.Use == "POWER")
                                .Select(p => p.Name).ToList();
                            var groundPins = pins.Where(p => p.Use == "GROUND")
                                .Select(p => p.Name).ToList();

                            macroInfos.Add(new MacroInfo
                            {
                                Name = name,
                                Width = macroSize?.w,
                                Height = macroSize?.h,
                                PowerPins = powerPins,
                                GroundPins = groundPins
                            });

                        }
                        break;
                    case ProjectType.Digital:
                        break;
                    case ProjectType.Ip:
                        break;
                }

            }

            return additionalRefPath;
        }
    }
}
