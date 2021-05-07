﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Harbor.Commands.Project
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
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectType Type { get; set; }
        public List<string> StdCell { get; set; }
        public List<string> Io { get; set; }

        //References
        public List<ProjectReference> Reference { get; set; }

        //Memory
        public MemoryType? MemoryType { get; set; }
        public MemoryPortType? MemoryPortType { get; set; }
        public int? Words { get; set; }
        public int? Bits { get; set; }

        public static async Task<ProjectInfo> ReadFromCurrentDirectoryAsync()
        {
            var json = await File.ReadAllTextAsync(Path.Combine(Environment.CurrentDirectory, "project.json"));
            return JsonConvert.DeserializeObject<ProjectInfo>(json);
        }

        public static async Task<ProjectInfo> ReadFromDirectoryAsync(string path)
        {
            var json = await File.ReadAllTextAsync(Path.Combine(path, "project.json"));
            return JsonConvert.DeserializeObject<ProjectInfo>(json);
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
            var jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var pjInfoJson = JsonConvert.SerializeObject(this, Formatting.Indented, jsonSetting);
            await File.WriteAllTextAsync(Path.Combine(path, "project.json"),
                pjInfoJson, Encoding.UTF8);
        }
    }
}
