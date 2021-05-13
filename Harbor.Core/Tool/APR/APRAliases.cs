using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Harbor.Common.Project;
using Harbor.Core.Tool.AddPG;
using Harbor.Core.Tool.ConvertAMS;
using Harbor.Core.Tool.ConvertUpper;
using Harbor.Core.Tool.Formality;
using Harbor.Core.Tool.GetPorts;
using Harbor.Core.Tool.ICC;
using Harbor.Core.Tool.Sed;
using Harbor.Core.Util;
using Newtonsoft.Json.Linq;

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

            RunAPR(context, configure);
            RunAddPG(context, configure);
            RunConvertUpper(context, configure);
            RunConvertAMS(context, configure);
            RunFormality(context, configure);
        }

        [CakeMethodAlias]
        [CakeNamespaceImport("Harbor.Core.Tool.APR.Model")]
        public static void APR(this ICakeContext context, DirectoryPath project, APRRunnerSettings configure)
        {
            var absProject = context.MakeAbsolute(project);
            configure.ProjectPath = absProject;
            APR(context, configure);
        }

        [CakeMethodAlias]
        [CakeNamespaceImport("Harbor.Core.Tool.APR.Model")]
        public static void APR(this ICakeContext context, Action<APRRunnerSettingsBuilder> configure)
        {
            var builder = new APRRunnerSettingsBuilder(context);
            configure?.Invoke(builder);
            var absProject = context.MakeAbsolute(new DirectoryPath("./Layout"));
            builder.Settings.ProjectPath = absProject;
            APR(context, builder.Settings);
        }

        public static void RunAddPG(ICakeContext context, APRRunnerSettings settings)
        {
            if (settings.AddPG)
            {
                //在网表中删除wire only的单元
                var library = AllLibrary.GetLibrary(settings.ProjectInfo);
                if (library.PrimaryStdCell.wire_only_cells != null && library.PrimaryStdCell.wire_only_cells.Length != 0)
                {
                    foreach (var fill in library.PrimaryStdCell.wire_only_cells)
                    {
                        context.Sed(settings.ProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}_cds.v"), $"/{fill}/d");
                    }
                }
                //TODO WorkingDirectory = settings.ProjectPath.Combine("netlist")
                context.AddPG(AllLibrary.GetLibrary(settings.ProjectInfo), settings.ProjectInfo,
                    settings.ProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}_cds.v"),
                    settings.ProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}_cds_PG.v"),
                    settings.ProjectPath.Combine("netlist"));
            }
        }

        public static void RunConvertUpper(ICakeContext context, APRRunnerSettings settings)
        {
            //TODO WorkingDirectory = settings.ProjectPath.Combine("netlist")
            context.ConvertUpper(settings.Top,
                settings.SynProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}_combine.v"),
                settings.ProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}_cds_PG.v"), settings
                    .ProjectPath.Combine("netlist")
                    .CombineWithFilePath($"{settings.Top}_cds_func.v"),
                settings.ProjectPath.Combine("netlist"));
        }

        public static void RunConvertAMS(ICakeContext context, APRRunnerSettings settings)
        {
            //TODO　WorkingDirectory = settings.ProjectPath.Combine("netlist")
            context.ConvertAMS(settings.Top,
                settings.ProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}_cds_func.v"), settings
                    .ProjectPath.Combine("netlist")
                    .CombineWithFilePath($"{settings.Top}_cds_functional.v"),
                settings.ProjectPath.Combine("netlist"));
        }


        public static void RunAPR(ICakeContext context, APRRunnerSettings settings)
        {
            settings.GenerateTclScripts();
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

        public static void RunFormality(ICakeContext context, APRRunnerSettings settings)
        {
            if (settings.FormalVerify)
            {
                var configure = new FormalityRunnerSettings
                {
                    APRSettings = settings
                };
                context.Formality(configure);
            }
        }

    }
}
