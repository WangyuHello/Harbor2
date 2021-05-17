using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Harbor.Common.Project;

namespace Harbor.Core.Tool.AddPG
{
    public static class AddPGAliases
    {
        [CakeMethodAlias]
        public static void AddPG(this ICakeContext context, Library library, ProjectInfo projectInfo, FilePath filename, FilePath output, DirectoryPath workingDirectory)
        {
            context.Log.Verbose("运行AddPg");
            Harbor.Python.Tool.AddPg.Run2(library, projectInfo, filename.FullPath, output.FullPath, workingDirectory.FullPath);
        }
    }
}
