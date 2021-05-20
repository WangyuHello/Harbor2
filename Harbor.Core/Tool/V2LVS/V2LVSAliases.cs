using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Harbor.Core.Tool.V2LVS
{
    public static class V2LVSAliases
    {
        [CakeMethodAlias]
        public static void V2LVS(this ICakeContext context, V2LVSRunnerSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new V2LVSRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(settings, context);
        }
    }
}
