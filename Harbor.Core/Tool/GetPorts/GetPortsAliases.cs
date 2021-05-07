using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.Core.Tool.GetPorts
{
    public static class GetPortsAliases
    {
        [CakeMethodAlias]
        public static void GetPorts(this ICakeContext context, string top, string file)
        {
            var configure = new GetPortsRunnerSettings
            {
                Top = top,
                File = file
            };
            GetPorts(context, configure);
        }

        [CakeMethodAlias]
        public static void GetPorts(this ICakeContext context, GetPortsRunnerSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new GetPortsRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(settings);
        }
    }
}
