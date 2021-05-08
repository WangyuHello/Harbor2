using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core;

namespace Harbor.Core.Tool.LC
{
    public static class LCAliases
    {
        [CakeMethodAlias]
        public static void LC(this ICakeContext context, LCRunnerSettings configure)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new LCRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(configure, context);
        }
    }
}
