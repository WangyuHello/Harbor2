using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core;

namespace Harbor.Core.Tool.Milkyway
{
    public static class MilkywayAliases
    {
        [CakeMethodAlias]
        public static void Milkyway(this ICakeContext context, MilkywayRunnerSettings configure)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            configure.Context = context;
            var runner = new MilkywayRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(configure, context);
        }
    }
}
