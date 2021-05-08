using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Cake.Common.IO;
using Cake.Core.IO;
using Cake.FileHelpers;
using Harbor.Core.Project;
using Harbor.Core.Tool.Ihdl;
using Harbor.Core.Tool.Project;
using Harbor.Core.Tool.StrmIn;
using Harbor.Core.Tool.Virtuoso;
using Harbor.Core.Util;

namespace Harbor.Core.Tool.Cadence
{
    public static class CadenceAliases
    {
        [CakeMethodAlias]
        public static void CreateCadenceProject(this ICakeContext context, CadenceRunnerSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new CadenceRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(settings, context);
        }

        [CakeMethodAlias]
        public static void CreateCadenceProject(this ICakeContext context)
        {
            //var settings = new CadenceRunnerSettings();
            //CreateCadenceProject(context, settings);

            var projectInfo = ProjectInfo.ReadFromContext(context);
            var library = AllLibrary.GetLibrary(projectInfo);

            var cadenceDir = context.Environment.WorkingDirectory.Combine("Cadence");
            var cdsDir = cadenceDir.Combine(projectInfo.Project);
            if (context.DirectoryExists(cadenceDir))
            {
                context.DeleteDirectory(cadenceDir,
                    new DeleteDirectorySettings {Force = true, Recursive = true});
            }

            context.CreateDirectory(cadenceDir);
            context.CreateAnalogProject(cadenceDir, projectInfo.Project, library.Pdk, library.StdCell, library.Io);
            projectInfo.Type = ProjectType.Analog;
            projectInfo.WriteToDirectoryAsync(cdsDir.FullPath).Wait();
            CdsUtil.RefreshCdsLibAsync(cdsDir.FullPath, projectInfo).Wait();

            var vName = context.Environment.WorkingDirectory.CombineWithFilePath(
                $"./Layout/netlist/{projectInfo.Project}_cds_PG.v");
            var funcvName = context.Environment.WorkingDirectory.CombineWithFilePath(
                $"./Layout/netlist/{projectInfo.Project}_cds_functional.v");
            var gdsName = context.Environment.WorkingDirectory.CombineWithFilePath(
                $"./Layout/gds/{projectInfo.Project}.gds");

            //AutoImport
            context.AutoImport(cdsDir, vName, funcvName, gdsName, projectInfo.Project, projectInfo.Project);
        }

        [CakeMethodAlias]
        public static void AutoImport(this ICakeContext context, DirectoryPath directory, FilePath vName, FilePath funcvName, FilePath gdsName, string libName, string topCellName)
        {
            var projectInfo = ProjectInfo.ReadFromDirectory(directory.FullPath);
            var library = AllLibrary.GetLibrary(projectInfo);
            var libraryName = projectInfo.Library;
            var m2Width = library.PrimaryStdCell.m2_width;
            var m1RoutingDirection = library.PrimaryStdCell.m1_routing_direction;

            var pgTemplate = "";
            

            var addPinTemplate = libraryName switch
            {
                string l when l.StartsWith("TSMC") => $"auto_add_pin_tsmc(\"{libName}\" \"{topCellName}\" {m2Width})",
                string l when l.StartsWith("HL") => $"auto_add_pin_hl(\"{libName}\" \"{topCellName}\" {m2Width})",
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
                string l when l.StartsWith("SMIC40HV") => $"auto_wire_lib(\"{libName}\")",
                _ => ""
            };

            var postTemplate = libraryName switch
            {
                string l when l.StartsWith("SMIC40HV") => $"auto_add_dnw(\"{libName}\" \"{topCellName}\")",
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
                CommandLogFile = directory.CombineWithFilePath($"./.harbor/{topCellName}.virtuoso.log")
            });
        }
    }
}
