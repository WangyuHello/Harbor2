using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.PrimeTime
{
    public static class PrimeTimeAliases
    {
        [CakeMethodAlias]
        public static void PrimeTime(this ICakeContext context, PrimeTimeRunnerSettings configure)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new PrimeTimeRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(configure, context);
        }

        [CakeMethodAlias]
        public static void PrimeTime(this ICakeContext context, string commandFile)
        {
            var configure = new PrimeTimeRunnerSettings
            {
                CommandFile = commandFile // -file xxx.tcl
            };

            PrimeTime(context, configure);
        }
    }
}
