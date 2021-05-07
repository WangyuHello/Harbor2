using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

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
            runner.Run(settings);
        }

        [CakeMethodAlias]
        public static void CreateCadenceProject(this ICakeContext context)
        {
            var settings = new CadenceRunnerSettings();
            CreateCadenceProject(context, settings);
        }
    }
}
