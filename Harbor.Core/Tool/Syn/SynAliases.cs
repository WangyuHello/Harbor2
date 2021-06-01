using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cake.Common.IO;
using Harbor.Common.Project;
using Harbor.Core.Tool.AddPG;
using Harbor.Core.Tool.DC;
using Harbor.Core.Tool.PrimeTime;


namespace Harbor.Core.Tool.Syn
{
    public static class SynAliases
    {

        private static void Syn(this ICakeContext context, SynRunnerSettings configure)
        {
            configure.Context = context ?? throw new ArgumentNullException(nameof(context));
            RunDC(context, configure);
            RunCombineSource(context, configure);
            RunAddPG(context, configure);
            RunSTA(context, configure);
        }

        [CakeMethodAlias]
        public static void Syn(this ICakeContext context, DirectoryPath project, SynRunnerSettings configure)
        {
            var absProject = context.MakeAbsolute(project);
            configure.ProjectPath = absProject;
            Syn(context, configure);
        }

        [CakeMethodAlias]
        public static void Syn(this ICakeContext context, DirectoryPath _, Action<SynRunnerSettingsBuilder> configure)
        {
            Syn(context, configure);
        }

        [CakeMethodAlias]
        public static void Syn(this ICakeContext context, Action<SynRunnerSettingsBuilder> configure)
        {
            var builder = new SynRunnerSettingsBuilder(context);
            configure?.Invoke(builder);
            Syn(context, "./Synthesis", builder.Settings);
        }

        private static void RunDC(ICakeContext context, SynRunnerSettings settings)
        {
            settings.GenerateRunScripts();
            context.DC(settings.GetDcRunnerSettings(), SynExitHandler.Handle);
        }

        private static void RunAddPG(ICakeContext context, SynRunnerSettings settings)
        {
            if (!settings.AddPG) return;
            var library = AllLibrary.GetLibrary(settings.ProjectInfo);
            context.AddPG(AllLibrary.GetLibrary(settings.ProjectInfo), settings.ProjectInfo,
                settings.ProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}.v"),
                settings.ProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}_PG.v"),
                library.PrimaryStdCell.wire_only_cells,
                settings.ProjectPath.Combine("netlist"));
        }

        private static void RunSTA(ICakeContext context, SynRunnerSettings settings)
        {
            if (settings.STA)
            {
                var configure = new PrimeTimeRunnerSettings
                {
                    SynSettings = settings
                };
                context.PrimeTime(configure);
            }
        }

        private static void RunCombineSource(ICakeContext context, SynRunnerSettings settings)
        {
            var sources = settings.Verilog;
            var output = settings.ProjectPath.Combine("netlist").CombineWithFilePath($"{settings.Top}_combine.v");

            StringBuilder sb = new StringBuilder();
            foreach (var source in sources)
            {
                sb.Append(File.ReadAllText(source.FullPath));
                sb.AppendLine();
            }

            var f = new StreamWriter(File.Open(output.FullPath, FileMode.Create, FileAccess.Write));
            f.WriteLine(sb);
            f.Close();
        }
    }
}
