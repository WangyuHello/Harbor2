using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core.IO;

namespace Harbor.Core.Tool.ConvertUpper
{
    public static class ConvertUpperAliases
    {
        [CakeMethodAlias]
        public static void ConvertUpper(this ICakeContext context, string top, FilePath source, FilePath netlist, FilePath output, DirectoryPath workingDirectory)
        {
            Harbor.Python.Tool.ConvertUpper.Run2(top, source.FullPath, netlist.FullPath, output.FullPath, workingDirectory.FullPath);
        }

    }
}
