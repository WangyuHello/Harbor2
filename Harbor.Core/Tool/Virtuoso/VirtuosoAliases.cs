using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.Virtuoso
{
    public static class VirtuosoAliases
    {
        [CakeMethodAlias]
        public static void Virtuoso(this ICakeContext context, VirtuosoRunnerSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new VirtuosoRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(settings, context);
        }
    }
}
