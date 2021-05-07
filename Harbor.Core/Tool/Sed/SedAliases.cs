using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core.IO;

namespace Harbor.Core.Tool.Sed
{
    public static class SedAliases
    {
        [CakeMethodAlias]
        public static void Sed(this ICakeContext context, FilePath file, string command)
        {
            var settings = new SedRunnerSettings
            {
                File = file,
                Command = command
            };
            Sed(context, settings);
        }

        [CakeMethodAlias]
        public static void Sed(this ICakeContext context, SedRunnerSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new SedRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(settings);
        }
    }
}
