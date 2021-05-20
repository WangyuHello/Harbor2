using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Harbor.Common.Project;

namespace Harbor.Core
{
    public abstract class HarborToolSettings : ToolSettings
    {
        public FilePath CommandLogFile { get; set; }
        public ICakeContext Context { get; set; }
        private ProjectInfo _projectInfo;

        public ProjectInfo ProjectInfo => _projectInfo ??= ProjectInfo.ReadFromContext(Context);

        protected HarborToolSettings(){}

        protected HarborToolSettings(ICakeContext context)
        {
            Context = context;
        }

        internal virtual void Evaluate(ProcessArgumentBuilder args)
        {
            
        }

        internal virtual void GenerateRunScripts() { }
    }

    public static class ProcessArgumentBuilderExtensions
    {
        public static void Append(this ProcessArgumentBuilder arg, string option, string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                arg.Append($"-{option} {str}");
            }
        }

        public static void Append(this ProcessArgumentBuilder arg, string option, FilePath file)
        {
            if (file != null)
            {
                arg.Append($"-{option} {file.FullPath}");
            }
        }

        public static void Append(this ProcessArgumentBuilder arg, string option, FilePathCollection files)
        {
            if (files == null) return;
            foreach (var file in files)
            {
                arg.Append($"-{option} {file.FullPath}");
            }
        }

        public static void Append(this ProcessArgumentBuilder arg, string option, bool sw)
        {
            if (sw)
            {
                arg.Append($"-{option}");
            }
        }

        public static void Append<T>(this ProcessArgumentBuilder arg, string option, T e) where T : Enum
        {
            if (e != null)
            {
                arg.Append($"-{option} {Enum.GetName(typeof(T), e)}");
            }
        }
    }
}
