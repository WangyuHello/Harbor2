using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.IO;

namespace Harbor.Commands.Util
{
    public static class RelativePathExtension
    {
        public static string GetRelativePath(this string path)
        {
            DirectoryPath dPath = new DirectoryPath(path);
            var relativePath = dPath.GetRelativePath(new DirectoryPath(System.Environment.CurrentDirectory));
            return relativePath.ToString();
        }
    }
}
