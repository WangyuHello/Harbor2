using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Harbor.Common.Util;
using Mono.Unix;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Path = System.IO.Path;
// ReSharper disable UnusedMember.Global

namespace Harbor.Common.Project
{
    public sealed class ProjectReference
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public sealed class ProjectInfo
    {
        public string Project { get; set; }
        public string Library { get; set; }
        [JsonProperty("ProjectType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectType Type { get; set; }
        [JsonConverter(typeof(SingleOrArrayConverter<string>))]
        public List<string> StdCell { get; set; }
        [JsonConverter(typeof(SingleOrArrayConverter<string>))]
        public List<string> Io { get; set; }

        public string GetPrimaryStdCell() => StdCell.First();

        //References
        public List<ProjectReference> Reference { get; set; }

        //Memory
        public MemoryType? MemoryType { get; set; }
        public MemoryPortType? MemoryPortType { get; set; }
        public int? Words { get; set; }
        public int? Bits { get; set; }

        private DirectoryPath _directory;
        /// <summary>
        /// projectInfo所在的目录绝对路径
        /// </summary>
        [JsonIgnore]
        public DirectoryPath Directory
        {
            get => _directory;
            set
            {
                if (value == _directory || value.IsRelative) return;

                if (Reference != null)
                {
                    foreach (var re in Reference)
                    {
                        var absDir = new DirectoryPath(re.Path).MakeAbsolute(_directory);
                        var targetRelativeDir = absDir.GetRelativePath(value);
                        re.Path = targetRelativeDir.FullPath;
                    }
                }

                _directory = value;
            }
        }

        public static async Task<ProjectInfo> ReadFromCurrentDirectoryAsync()
        {
            var json = await File.ReadAllTextAsync(Path.Combine(Environment.CurrentDirectory, "project.json"));
            var info = JsonConvert.DeserializeObject<ProjectInfo>(json);
            info!._directory = Environment.CurrentDirectory;
            return info;
        }

        public static ProjectInfo ReadFromContext(ICakeContext context)
        {
            var json = File.ReadAllText(context.MakeAbsolute(new FilePath("project.json")).FullPath);
            var info = JsonConvert.DeserializeObject<ProjectInfo>(json);
            info!._directory = context.Environment.WorkingDirectory;
            return info;
        }

        public static async Task<ProjectInfo> ReadFromDirectoryAsync(string path)
        {
            var dir = new DirectoryPath(path);
            if (dir.IsRelative)
            {
                dir = dir.MakeAbsolute(Environment.CurrentDirectory);
            }
            dir = new DirectoryPath(UnixPath.GetCompleteRealPath(dir.FullPath));
            var json = await File.ReadAllTextAsync(dir.CombineWithFilePath("project.json").FullPath);
            var info = JsonConvert.DeserializeObject<ProjectInfo>(json);
            info!._directory = dir;
            return info;
        }

        public static ProjectInfo ReadFromDirectory(string path)
        {
            var dir = new DirectoryPath(path);
            if (dir.IsRelative)
            {
                dir = dir.MakeAbsolute(Environment.CurrentDirectory);
            }
            dir = new DirectoryPath(UnixPath.GetCompleteRealPath(dir.FullPath));
            var json = File.ReadAllText(dir.CombineWithFilePath("project.json").FullPath);
            var info = JsonConvert.DeserializeObject<ProjectInfo>(json);
            info!._directory = dir;
            return info;
        }

        public static ProjectInfo ReadFromDirectory(DirectoryPath path)
        {
            path = new DirectoryPath(UnixPath.GetCompleteRealPath(path.FullPath));
            var json = File.ReadAllText(path.CombineWithFilePath("project.json").FullPath);
            var info = JsonConvert.DeserializeObject<ProjectInfo>(json);
            info!._directory = path;
            return info;
        }

        public async Task WriteToCurrentDirectoryAsync()
        {
            var jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var pjInfoJson = JsonConvert.SerializeObject(this, Formatting.Indented, jsonSetting);
            await File.WriteAllTextAsync(Path.Combine(Environment.CurrentDirectory, "project.json"),
                pjInfoJson, Encoding.UTF8);
        }

        public async Task WriteToDirectoryAsync(string path)
        {
            Directory = path;
            var jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var pjInfoJson = JsonConvert.SerializeObject(this, Formatting.Indented, jsonSetting);
            await File.WriteAllTextAsync(Path.Combine(path, "project.json"),
                pjInfoJson, Encoding.UTF8);
        }

        public async Task Write()
        {
            var jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var pjInfoJson = JsonConvert.SerializeObject(this, Formatting.Indented, jsonSetting);
            await File.WriteAllTextAsync(Directory.CombineWithFilePath("project.json").FullPath,
                pjInfoJson, Encoding.UTF8);
        }

    }
}
