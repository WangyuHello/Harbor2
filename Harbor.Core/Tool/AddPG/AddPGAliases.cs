using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.AddPG
{
    public static class AddPGAliases
    {
        [CakeMethodAlias]
        public static void AddPG(this ICakeContext context, string library, string stdCell, string file)
        {
            var configure = new AddPGRunnerSettings
            {
                Library = library,
                StdCell = stdCell,
                File = file
            };

            AddPG(context, configure);
        }

        [CakeMethodAlias]
        public static void AddPG(this ICakeContext context, AddPGRunnerSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new AddPGRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(settings, context);
        }
    }
}
