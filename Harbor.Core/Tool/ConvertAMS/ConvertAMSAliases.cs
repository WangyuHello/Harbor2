using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.ConvertAMS
{
    public static class ConvertAMSAliases
    {
        [CakeMethodAlias]
        public static void ConvertAMS(this ICakeContext context, string top, string srcfile, string outputfile)
        {
            var configure = new ConvertAMSRunnerSettings
            {
                Top = top,
                SourceFile = srcfile,
                OutputFile = outputfile
            };
            ConvertAMS(context, configure);
        }

        [CakeMethodAlias]
        public static void ConvertAMS(this ICakeContext context, ConvertAMSRunnerSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new ConvertAMSRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(settings);
        }
    }
}
