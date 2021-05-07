using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Harbor.Core
{
    public abstract class HarborTool<TSettings> :Tool<TSettings> where TSettings : HarborToolSettings
    {
        protected HarborTool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {

        }

        protected static ProcessArgumentBuilder GetSettingsArguments(TSettings settings)
        {
            var args = new ProcessArgumentBuilder();
            settings?.Evaluate(args);
            return args;
        }

        public void Run(TSettings settings, Action<int> exitHandler)
        {
            exitCodeHandler = exitHandler;
            Run(settings);
        }

        public void Run(TSettings settings)
        {
            settings.GenerateTclScripts();
            var args = GetSettingsArguments(settings);

            PrintBanner(settings, args);
            Run(settings, args, new ProcessSettings
                {
                    RedirectStandardOutput = true,
                    RedirectedStandardOutputHandler = _ =>
                    {
                        Console.WriteLine(_);
                        return _;
                    } //实时更新Output
                },
                process =>
                {
                    //程序运行完一并获取Output
                    if (settings.CommandLogFile != null)
                    {
                        File.WriteAllLines(GetWorkingDirectory(settings).CombineWithFilePath(settings.CommandLogFile).FullPath, process.GetStandardOutput());
                    }
                });
        }

        protected void PrintBanner(TSettings settings, ProcessArgumentBuilder args)
        {
            var dash = string.Join("", Enumerable.Range(0, Console.WindowWidth).Select(_ => "-"));
            Console.WriteLine(dash);
            Console.WriteLine($"{GetToolName()} 运行目录: {GetWorkingDirectory(settings)}");
            Console.WriteLine($"{GetToolName()} 运行参数: {args.RenderSafe()}");
            Console.WriteLine(dash);
        }

        private Action<int> exitCodeHandler;

        protected override void ProcessExitCode(int exitCode)
        {
            if (exitCodeHandler != null)
            {
                exitCodeHandler(exitCode);
            }
            else
            {
                base.ProcessExitCode(exitCode);
            }
        }
    }
}
