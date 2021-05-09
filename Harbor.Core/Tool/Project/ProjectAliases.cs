using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Harbor.Common.Project;
using Harbor.Core.Util;

namespace Harbor.Core.Tool.Project
{
    public static class ProjectAliases
    {
        [CakeMethodAlias]
        public static ProjectInfo ReadProject(this ICakeContext context)
        {
            return ProjectInfo.ReadFromContext(context);
        }

        [CakeMethodAlias]
        public static void CreateAnalogProject(this ICakeContext context, DirectoryPath baseDir, string name, Library.LibraryPdk pdk, List<Library.LibraryStdCell> stdCell, List<Library.LibraryIo> io)
        {
            var dir = baseDir.Combine(name);
            if (context.DirectoryExists(dir))
            {
                context.DeleteDirectory(dir, new DeleteDirectorySettings {Force = true, Recursive = true});
            }
            context.CreateDirectory(dir);
            context.CreateDirectory(dir.Combine(".harbor"));
            CdsUtil.CreateCdsLibAsync(dir.FullPath, pdk, stdCell, io).Wait();
        }
    }
}
