using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.ICC
{
    public static class ICCAliases
    {
        [CakeMethodAlias]
        public static void ICC(this ICakeContext context, ICCRunnerSettings configure)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new ICCRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(configure, context);
        }

        [CakeMethodAlias]
        public static void ICC(this ICakeContext context, string commandFile)
        {
            var configure = new ICCRunnerSettings
            {
                CommandFile = commandFile // -f xxx.tcl
            };

            ICC(context, configure);
        }

        [CakeMethodAlias]
        public static void ICC(this ICakeContext context, ICCRunnerSettings configure, Action<int> exitHandler)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new ICCRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(configure, exitHandler, context);
        }
    }
}
