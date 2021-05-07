using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Cake.Core.IO;

namespace Harbor.Core.Util
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    static class IOHelper
    {
        public static void CreateDirectory(DirectoryPath dir)
        {
            CreateDirectory(dir.FullPath);
        }

        public static void CreateDirectory(string fullpath)
        {
            if (!Directory.Exists(fullpath))
            {
                Directory.CreateDirectory(fullpath);
            }
        }

        public static void DeleteDirectory(string fullpath)
        {
            if (Directory.Exists(fullpath))
            {
                Directory.Delete(fullpath, true);
            }
        }

        public static void DeleteDirectory(DirectoryPath workingDirectory)
        {
            if (Directory.Exists(workingDirectory.FullPath))
            {
                Directory.Delete(workingDirectory.FullPath, true);
            }
        }
    }
}
