using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.StrmIn
{
    public static class StrmInAliases
    {

        [CakeMethodAlias]
        public static void StrmIn(this ICakeContext context, StrmInRunnerSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new StrmInRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(settings);
        }
    }
}
