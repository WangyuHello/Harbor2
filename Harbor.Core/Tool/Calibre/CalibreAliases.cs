using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
// ReSharper disable InconsistentNaming

namespace Harbor.Core.Tool.Calibre
{
    public static class CalibreAliases
    {
        public static void CalibreLVS(this ICakeContext context, FilePath gdsFile, string primaryCell, FilePath netlistFile)
        {
            var settings = new CalibreLVSSettings(context)
            {
                GDSFile = gdsFile,
                PrimaryCell = primaryCell,
                NetlistFile = netlistFile
            };
            settings.GenerateRunScripts();
            context.Calibre(settings.ToCalibreRunnerSettings());
        }

        [CakeMethodAlias]
        public static void Calibre(this ICakeContext context, CalibreRunnerSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new CalibreRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(settings, context);
        }
    }
}
