using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.FileHelpers;
using Harbor.Core.Environment;
using Spectre.Console;

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

        public void Run(TSettings settings, Action<int> exitHandler, ICakeContext context)
        {
            _exitCodeHandler = exitHandler;
            Run(settings, context);
        }

        public void Run(TSettings settings, ICakeContext context)
        {
            settings.Context = context;
            settings.GenerateRunScripts();
            //settings.EnvironmentVariables = EnvironmentHelper.AddEnvironment(GetToolExecutableNames(), settings.EnvironmentVariables);
            EnvironmentHelper.SetEnvironment(GetToolExecutableNames().First());
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
                        context.FileWriteLines(GetWorkingDirectory(settings).CombineWithFilePath(settings.CommandLogFile), process.GetStandardOutput().ToArray());
                    }
                });
        }

        protected void PrintBanner(TSettings settings, ProcessArgumentBuilder args)
        {
            AnsiConsole.Render(new Rule($"{GetToolName()}") {Alignment = Justify.Center, Style = Style.Parse("blue dim") });
            var table = new Table();
            table.AddColumn("项");
            table.AddColumn("值");
            table.AddRow("运行目录", GetWorkingDirectory(settings).ToString());
            table.AddRow("运行参数", args.RenderSafe());
            //foreach (var env in settings.EnvironmentVariables)
            //{
            //    table.AddRow(env.Key, env.Value);
            //}
            //table.AddRow("PATH", System.Environment.GetEnvironmentVariable("PATH")!);
            table.Expand();
            AnsiConsole.Render(table);
            AnsiConsole.Render(new Rule(){ Style = Style.Parse("blue dim") });
        }

        private Action<int> _exitCodeHandler;

        protected override void ProcessExitCode(int exitCode)
        {
            if (_exitCodeHandler != null)
            {
                _exitCodeHandler(exitCode);
            }
            else
            {
                base.ProcessExitCode(exitCode);
            }
        }
    }
}
