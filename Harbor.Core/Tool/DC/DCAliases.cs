using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.DC
{
    public static class DCAliases
    {

        [CakeMethodAlias]
        public static void DC(this ICakeContext context, DCRunnerSettings configure)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new DCRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(configure);
        }

        [CakeMethodAlias]
        public static void DC(this ICakeContext context, string command_file)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new DCRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            var configure = new DCRunnerSettings
            {
                CommandFile = command_file // -f xxx.tcl
            };

            runner.Run(configure);
        }

        [CakeMethodAlias]
        public static void DC(this ICakeContext context, DCRunnerSettings configure, Action<int> exitHandler)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new DCRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(configure, exitHandler);
        }
    }
}
