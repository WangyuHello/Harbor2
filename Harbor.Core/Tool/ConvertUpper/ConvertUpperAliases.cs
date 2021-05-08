using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.ConvertUpper
{
    public static class ConvertUpperAliases
    {
        [CakeMethodAlias]
        public static void ConvertUpper(this ICakeContext context, string top, string srcfile, string netlistfile, string outputfile)
        {
            var configure = new ConvertUpperRunnerSettings
            {
                Top = top,
                SourceFile = srcfile,
                NetlistFile = netlistfile,
                OutputFile = outputfile
            };
            ConvertUpper(context, configure);
        }

        [CakeMethodAlias]
        public static void ConvertUpper(this ICakeContext context, ConvertUpperRunnerSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new ConvertUpperRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(settings, context);
        }
    }
}
