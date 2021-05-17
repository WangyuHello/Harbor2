using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core.IO;

namespace Harbor.Core.Tool.ConvertAMS
{
    public static class ConvertAMSAliases
    {
        [CakeMethodAlias]
        public static void ConvertAMS(this ICakeContext context, string top, FilePath source, FilePath output, DirectoryPath workingDirectory)
        {
            Harbor.Python.Tool.ConvertAMS.Run2(top, source.FullPath, output.FullPath, workingDirectory.FullPath);
        }
    }
}
