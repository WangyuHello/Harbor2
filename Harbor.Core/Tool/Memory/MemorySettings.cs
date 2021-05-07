using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Cake.Core;
using Harbor.Core;
using Harbor.Core.Tool.LC;
using Harbor.Core.Tool.Milkyway;
using Harbor.Core.Util;

namespace Harbor.Core.Tool.Memory
{
    public class MemorySettings : HarborToolSettings
    {
        public enum MemoryPortType
        {
            Single,
            Dual,
            Two
        }

        public class MemoryDefinition
        {
            public int Word { get; set; }
            public int Bit { get; set; }
            public int YMux { get; set; }
            public string Name { get; set; }
            public MemoryPortType PortType { get; set; }
        }

        public List<MemoryDefinition> Memory { get; set; }

        public string FindCorrectMemoryCompiler(MemoryPortType type)
        {
            return null;
        }

        public void LefToFram(string workingDirectory)
        {
            Console.WriteLine("Converting LEF to Milkyway libs");
            var lefs = Directory.GetFiles(workingDirectory, "*.lef", SearchOption.TopDirectoryOnly);
            foreach (var lef in lefs)
            {
                var projectName = System.IO.Path.GetFileNameWithoutExtension(lef);
                Context.Milkyway(new MilkywayRunnerSettings
                {
                    ProjectInfo = ProjectInfo,
                    WorkingDirectory = workingDirectory,
                    ProjectLefFilePath = lef,
                    ProjectName = projectName
                });
            }
        }

        public void LibToDb(string workingDirectory)
        {
            Console.WriteLine("Converting lib to db");
            var libs = Directory.GetFiles(workingDirectory, "*.lib", SearchOption.TopDirectoryOnly);
            foreach (var lib in libs)
            {
                var projectName = System.IO.Path.GetFileNameWithoutExtension(lib);
                Context.LC(new LCRunnerSettings
                {
                    WorkingDirectory = workingDirectory,
                    ProjectName = projectName,
                    ProjectLibFilePath = lib
                });
            }
        }

        /// <summary>
        /// 使用MemoryCompiler生成
        /// </summary>
        public void GenerateMemory(string memPath)
        {
            IOHelper.DeleteDirectory(memPath);
            IOHelper.CreateDirectory(memPath);

            if (Memory.Count == 0)
            {
                return;
            }
            foreach (var mem in Memory)
            {
                var memCompiler = FindCorrectMemoryCompiler(mem.PortType);
                if (string.IsNullOrEmpty(memCompiler))
                {
                    continue;
                }
                string args = $"{memCompiler} -bits {mem.Bit} -words {mem.Word} -mux {mem.YMux} -lef -lib -v -gds -cdl -mbist -savepath {memPath}";
                if (!string.IsNullOrEmpty(mem.Name))
                {
                    args += $" -instname {mem.Name}";
                }
                Console.WriteLine($"Compiling Memory {mem.Name}");
                ProcessStartInfo psi = new ProcessStartInfo("java", "-jar " + args)
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = memPath
                };
                var p = Process.Start(psi);
                p?.WaitForExit();
            }
            LefToFram(memPath);
            LibToDb(memPath);
        }
    }

    public class MemorySettingsBuilder
    {
        public MemorySettings Settings { get; } = new MemorySettings();

        public MemorySettingsBuilder AddMemory(MemorySettings.MemoryPortType portType, int word, int bit, int ymux, string name = "")
        {
            Settings.Memory.Add(new MemorySettings.MemoryDefinition
            {
                Word = word,
                Bit = bit,
                YMux = ymux,
                Name = name,
                PortType = portType
            });
            return this;
        }

        public MemorySettingsBuilder AddDualPortSRAM(int word, int bit, int ymux, string name = "")
        {
            return this.AddMemory(MemorySettings.MemoryPortType.Dual, word, bit, ymux, name);
        }

        public MemorySettingsBuilder AddSinglePortSRAM(int word, int bit, int ymux, string name = "")
        {
            return this.AddMemory(MemorySettings.MemoryPortType.Single, word, bit, ymux, name);
        }

        public MemorySettingsBuilder AddTwoPortSRAM(int word, int bit, int ymux, string name = "")
        {
            return this.AddMemory(MemorySettings.MemoryPortType.Two, word, bit, ymux, name);
        }
    }
}
