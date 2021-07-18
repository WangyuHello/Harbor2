using Cake.Core.IO;

namespace Harbor.Commands.Util
{
    public static class RelativePathExtension
    {
        public static string GetRelativePath(this string path)
        {
            var dPath = new DirectoryPath(path);
            var relativePath = dPath.GetRelativePath(new DirectoryPath(System.Environment.CurrentDirectory));
            return relativePath.ToString();
        }
    }
}
