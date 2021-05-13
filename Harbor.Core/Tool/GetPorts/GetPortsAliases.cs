using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core.IO;
using Harbor.Python.Tool;

namespace Harbor.Core.Tool.GetPorts
{
    public static class GetPortsAliases
    {
        [CakeMethodAlias]
        public static List<VerilogPortDefinition> GetPorts(this ICakeContext context, string top, string file, DirectoryPath workingDirectory)
        {
            return Python.Tool.GetPorts.Run(file, top, workingDirectory.FullPath);
        }

    }
}
