using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.Formality
{
    public static class FormalityAliases
    {
        [CakeMethodAlias]
        public static void Formality(this ICakeContext context, FormalityRunnerSettings configure)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new FormalityRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(configure);
        }

        [CakeMethodAlias]
        public static void Formality(this ICakeContext context, string commandFile)
        {
            var configure = new FormalityRunnerSettings
            {
                CommandFile = commandFile // -file xxx.tcl
            };
            Formality(context, configure);
        }
    }
}
