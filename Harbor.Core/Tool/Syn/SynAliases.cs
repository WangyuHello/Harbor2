using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Harbor.Common.Project;
using Harbor.Core.Tool.AddPG;
using Harbor.Core.Tool.DC;
using Harbor.Core.Tool.PrimeTime;
using Harbor.Core.Util;


namespace Harbor.Core.Tool.Syn
{
    public static class SynAliases
    {
        private static void Syn(this ICakeContext context, SynRunnerSettings configure)
        {
            configure.Context = context ?? throw new ArgumentNullException(nameof(context));
            var (match, newHash) = CheckHash(context, configure);
            if (!match)
            {
                RunDC(context, configure);
                RunCombineSource(context, configure);
                RunAddPG(context, configure);
                RunSTA(context, configure);
            }
            else
            {
                context.Information("已是最新版本");
            }
            HashHelper.SaveLocalHash(newHash, "syn");
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
            var svs = sources.Where(s => s.GetExtension().Contains("sv")).ToList();
            if (svs.Count != 0)
            {
                return; //SystemVerilog暂不支持
            }
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

        private static (bool match, Dictionary<string, string> newHash) CheckHash(ICakeContext context, SynRunnerSettings settings)
        {
            var fl = new List<string>();
            fl.AddRange(settings.Verilog.Select(f => f.FullPath));
            fl.Add(context.MakeAbsolute(new FilePath("build.cake")).FullPath);
            fl.AddRange(settings.AdditionalTimingDb.Select(f => f.FullPath));

            return HashHelper.Check(fl, "syn");
        }
    }
}
