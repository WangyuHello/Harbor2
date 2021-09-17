using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Harbor.Common.Project;
using Harbor.Core.Tool.AddPG;
using Harbor.Core.Tool.Calibre;
using Harbor.Core.Tool.Formality;
using Harbor.Core.Tool.ICC;
using Harbor.Core.Tool.V2LVS;
using Harbor.Core.Util;
using Spectre.Console;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace Harbor.Core.Tool.APR
{
    public static class APRAliases
    {
        /// <summary>
        /// 运行自动布局布线
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configure"></param>
        private static void APR(this ICakeContext context, APRRunnerSettings configure)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (configure.FloorPlanOnly)
            {
                AnsiConsole.MarkupLine("FloorPlan Only");
                RunAPR(context, configure);
            }
            else
            {
                var (match, newHash) = CheckHash(context, configure);
                if (!match)
                {
                    RunAPR(context, configure);
                    RunAddPG(context, configure);
                    RunFormality(context, configure);
                    RunLVS(context, configure);
                }
                else
                {
                    context.Information("已是最新版本");
                }
                HashHelper.SaveLocalHash(newHash, "apr");
            }
        }

        [CakeMethodAlias]
        [CakeNamespaceImport("Harbor.Common.Model")]
        public static void APR(this ICakeContext context, DirectoryPath project, APRRunnerSettings configure)
        {
            var absProject = context.MakeAbsolute(project);
            configure.ProjectPath = absProject;
            APR(context, configure);
        }

        [CakeMethodAlias]
        [CakeNamespaceImport("Harbor.Common.Model")]
        public static void APR(this ICakeContext context, DirectoryPath _, Action<APRRunnerSettingsBuilder> configure)
        {
            APR(context, configure);
        }

        [CakeMethodAlias]
        [CakeNamespaceImport("Harbor.Common.Model")]
        public static void APR(this ICakeContext context, Action<APRRunnerSettingsBuilder> configure)
        {
            var builder = new APRRunnerSettingsBuilder(context);
            configure?.Invoke(builder);
            var absProject = context.MakeAbsolute(new DirectoryPath("./Layout"));
            builder.Settings.ProjectPath = absProject;
            builder.Settings.SynProjectPath = context.MakeAbsolute(new DirectoryPath("./Synthesis"));
            APR(context, builder.Settings);
        }

        private static void RunAddPG(ICakeContext context, APRRunnerSettings settings)
        {
            if (!settings.AddPG) return;
            var library = AllLibrary.GetLibrary(settings.ProjectInfo);
            context.AddPG(AllLibrary.GetLibrary(settings.ProjectInfo), settings.ProjectInfo,
                settings.ProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}_cds.v"),
                settings.ProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}_cds_PG.v"),
                library.StdCell.SelectMany(std => std.wire_only_cells).ToArray(),
                settings.ProjectPath.Combine("netlist"));
        }

        private static void RunAPR(ICakeContext context, APRRunnerSettings settings)
        {
            settings.GenerateRunScripts();
            if (settings.UseICC)
            {
                context.ICC(settings.GetIccRunnerSettings(), APRExitHandler.HandleExit);
            }
            else if (settings.UseICC2)
            {
                
            }
            else if (settings.UseInnovus)
            {
                
            }
        }

        private static void RunFormality(ICakeContext context, APRRunnerSettings settings)
        {
            if (!settings.FormalVerify) return;
            var configure = new FormalityRunnerSettings
            {
                APRSettings = settings
            };
            context.Formality(configure);
        }

        private static void RunLVS(ICakeContext context, APRRunnerSettings settings)
        {
            if(!settings.LVS) return;
            var verilog = settings.ProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}_cds.v");
            var outputSpice = settings.ProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}.sp");
            var gds = settings.ProjectPath.Combine("gds").CombineWithFilePath($"{settings.Top}.gds");
            var library = AllLibrary.GetLibrary(settings.ProjectInfo);
            var libCdl = library.PrimaryStdCell.cdl_full_name;
            context.V2LVS(new V2LVSRunnerSettings
            {
                VerilogFile = verilog,
                OutputSpiceFile = outputSpice,
                SpiceLibraryFiles = new FilePathCollection{ libCdl }
            });
            context.CalibreLVS(gds, settings.Top, outputSpice);
        }

        private static (bool match, Dictionary<string, string> newHash) CheckHash(ICakeContext context, APRRunnerSettings settings)
        {
            var fl = new List<string>
            {
                settings.SynProjectPath.Combine("rpt").CombineWithFilePath($"{settings.Top}_area.rpt").FullPath,
                settings.SynProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}.v").FullPath,
                settings.SynProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}.sdc").FullPath,
                context.MakeAbsolute(new FilePath("build.cake")).FullPath
            };
            fl.AddRange(settings.AdditionalTimingDb.Select(f => f.FullPath));

            if (settings.PinSettings.ConstraintFile != null)
            {
                fl.Add(context.MakeAbsolute(settings.PinSettings.ConstraintFile).FullPath);
            }

            return HashHelper.Check(fl, "apr");
        }
    }
}
