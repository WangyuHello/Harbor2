using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core.IO;
using Cake.FileHelpers;
using Harbor.Common.Project;
using Harbor.Core.Tool.AddPG;
using Harbor.Core.Tool.ConvertAMS;
using Harbor.Core.Tool.ConvertUpper;
using Harbor.Core.Tool.Ihdl;
using Harbor.Core.Tool.Project;
using Harbor.Core.Tool.StrmIn;
using Harbor.Core.Tool.Virtuoso;
using Harbor.Core.Util;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace Harbor.Core.Tool.Cadence
{
    public static class CadenceAliases
    {
        [CakeMethodAlias]
        public static void CreateCadenceProject(this ICakeContext context)
        {
            var projectInfo = ProjectInfo.ReadFromContext(context);
            var (match, newHash) = CheckHash(context, projectInfo);
            var cadenceDir = context.MakeAbsolute(new DirectoryPath("./Cadence"));
            if (match && context.DirectoryExists(cadenceDir))
            {
                context.Information("已是最新版本");
                return;
            }

            var library = AllLibrary.GetLibrary(projectInfo);

            var cdsDir = cadenceDir.Combine(projectInfo.Project);
            if (context.DirectoryExists(cadenceDir))
            {
                context.DeleteDirectory(cadenceDir,
                    new DeleteDirectorySettings {Force = true, Recursive = true});
            }

            context.CreateDirectory(cadenceDir);
            context.CreateAnalogProject(cadenceDir, projectInfo.Project, library.Pdk, library.StdCell, library.Io);
            projectInfo.Type = ProjectType.Analog;
            projectInfo.Directory = cdsDir;
            projectInfo.Write().Wait();
            CdsUtil.RefreshCdsLibAsync(projectInfo).Wait();

            var vName = context.MakeAbsolute(new FilePath($"./Layout/netlist/{projectInfo.Project}_cds_PG.v"));
            var funcvName = context.MakeAbsolute(new FilePath($"./Layout/netlist/{projectInfo.Project}_cds_functional.v"));
            var gdsName = context.MakeAbsolute(new FilePath($"./Layout/gds/{projectInfo.Project}.gds"));

            RunAddPG(context, projectInfo, projectInfo.Project);
            RunConvertUpper(context, projectInfo, projectInfo.Project);
            RunConvertAMS(context, projectInfo, projectInfo.Project);
            //AutoImport
            context.AutoImport(cdsDir, vName, funcvName, gdsName, projectInfo.Project, projectInfo.Project);
            HashHelper.SaveLocalHash(newHash, "cds");
        }

        [CakeMethodAlias]
        public static void AutoImport(this ICakeContext context, DirectoryPath directory, FilePath vName, FilePath funcvName, FilePath gdsName, string libName, string topCellName)
        {
            var projectInfo = ProjectInfo.ReadFromDirectory(directory.FullPath);
            var library = AllLibrary.GetLibrary(projectInfo);
            var libraryName = projectInfo.Library;
            var m2Width = library.PrimaryStdCell.m2_width;
            var m1RoutingDirection = library.PrimaryStdCell.m1_routing_direction;

            string pgTemplate;
            
            var addPinTemplate = libraryName switch
            {
                { } l when l.StartsWith("TSMC") => $"auto_add_pin_tsmc(\"{libName}\" \"{topCellName}\" {m2Width})",
                { } l when l.StartsWith("HL") => $"auto_add_pin_hl(\"{libName}\" \"{topCellName}\" {m2Width})",
                _ => $"auto_add_pin(\"{libName}\" \"{topCellName}\" {m2Width})"
            };

            if (m1RoutingDirection == "vertical")
            {
                if (libraryName.StartsWith("HL"))
                {
                    pgTemplate = $"auto_add_pg_pin_text_h(\"{libName}\" \"{topCellName}\" \"text\" \"DVDD\" \"DVSS\")";
                }
                else
                {
                    pgTemplate = $"auto_add_pg_pin_h(\"{libName}\" \"{topCellName}\" \"DVDD\" \"DVSS\")";
                }
            }
            else
            {
                if (libraryName.StartsWith("HL"))
                {
                    pgTemplate = $"auto_add_pg_pin_text_v(\"{libName}\" \"{topCellName}\" \"text\" \"DVDD\" \"DVSS\")";
                }
                else
                {
                    pgTemplate = $"auto_add_pg_pin_v(\"{libName}\" \"{topCellName}\" \"DVDD\" \"DVSS\")";
                }
            }

            var preTemplate = libraryName switch
            {
                { } l when l.StartsWith("SMIC40HV") => $"auto_wire_lib(\"{libName}\")",
                _ => ""
            };

            var postTemplate = libraryName switch
            {
                { } l when l.StartsWith("SMIC40HV") => $"auto_add_dnw(\"{libName}\" \"{topCellName}\")",
                _ => ""
            };


            var ilTemplate = string.Join(Environment.NewLine, preTemplate, addPinTemplate, pgTemplate, postTemplate, "exit");
            var ilFile = directory.CombineWithFilePath($"./.harbor/{topCellName}.il");
            context.FileWriteText(ilFile, ilTemplate);

            context.ImportGDS(directory, gdsName, libName, topCellName);
            context.ImportVerilog(directory, vName, libName, topCellName);
            context.ImportVerilogFunctional(directory, funcvName, libName, topCellName);
            context.Virtuoso(new VirtuosoRunnerSettings
            {
                WorkingDirectory = directory,
                NoGraph = true,
                RestoreFile = ilFile,
                CommandLogFile = $"./.harbor/{topCellName}.virtuoso.log"
            });
        }


        public static void RunAddPG(ICakeContext context, ProjectInfo projectInfo, string top)
        {
            var layoutProjectPath = context.MakeAbsolute(new DirectoryPath("./Layout"));
            var library = AllLibrary.GetLibrary(projectInfo);
            context.AddPG(AllLibrary.GetLibrary(projectInfo), projectInfo,
                layoutProjectPath.Combine("netlist").CombineWithFilePath($"{top}_cds.v"),
                layoutProjectPath.Combine("netlist").CombineWithFilePath($"{top}_cds_PG.v"),
                library.StdCell.SelectMany(std => std.wire_only_cells).ToArray(),
                layoutProjectPath.Combine("netlist"));
            
        }

        public static void RunConvertUpper(ICakeContext context, ProjectInfo projectInfo, string top)
        {
            var layoutProjectPath = context.MakeAbsolute(new DirectoryPath("./Layout"));
            var synProjectPath = context.MakeAbsolute(new DirectoryPath("./Synthesis"));
            context.ConvertUpper(top,
                synProjectPath.Combine("netlist").CombineWithFilePath($"{top}_combine.v"),
                layoutProjectPath.Combine("netlist").CombineWithFilePath($"{top}_cds_PG.v"), 
                layoutProjectPath.Combine("netlist").CombineWithFilePath($"{top}_cds_func.v"),
                layoutProjectPath.Combine("netlist"));
        }

        public static void RunConvertAMS(ICakeContext context, ProjectInfo projectInfo, string top)
        {
            var layoutProjectPath = context.MakeAbsolute(new DirectoryPath("./Layout"));
            context.ConvertAMS(top,
                layoutProjectPath.Combine("netlist").CombineWithFilePath($"{top}_cds_func.v"), 
                layoutProjectPath.Combine("netlist").CombineWithFilePath($"{top}_cds_functional.v"),
                layoutProjectPath.Combine("netlist"));
        }

        private static (bool match, Dictionary<string, string> newHash) CheckHash(ICakeContext context, ProjectInfo projectInfo)
        {
            var fl = new List<string>
            {
                context.MakeAbsolute(new FilePath($"./Layout/netlist/{projectInfo.Project}_cds.v")).FullPath,
                context.MakeAbsolute(new FilePath($"./Synthesis/netlist/{projectInfo.Project}_combine.v")).FullPath,
                context.MakeAbsolute(new FilePath("build.cake")).FullPath
            };

            return HashHelper.Check(fl, "cds");
        }
    }
}
