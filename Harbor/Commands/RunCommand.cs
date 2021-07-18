using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Harbor.Commands.Environment;
using Harbor.Commands.Util;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Harbor.Commands
{
    public sealed class RunCommand : AsyncCommand<RunCommandSettings>
    {
        public override async Task<int> ExecuteAsync(CommandContext context, RunCommandSettings settings)
        {
            if (settings.Debug)
            {
                WaitForDebugger();
            }
            var app = settings.App;
            var version = settings.Version;
            if (EnvironmentHelper.SetEnvironment(app, version))
            {
                AnsiConsole.Render(new Rule(app.EscapeMarkup()) { Alignment = Justify.Center, Style = Style.Parse("blue dim") });
                var r = await SimpleRunner.Run(app, string.Join(" ", context.Remaining.Raw), redirectOutput: true);
                AnsiConsole.Render(new Rule { Style = Style.Parse("blue dim") });
                return r;
            }
            AnsiConsole.MarkupLine($"[red]不支持指定应用 {app.EscapeMarkup()}[/]");
            return -1;
        }

        private static void WaitForDebugger()
        {
            var processId = System.Environment.ProcessId;
            AnsiConsole.MarkupLine("等待调试器连接, 进程号: " + processId);

            Task.Run(() =>
            {
                while (!Debugger.IsAttached)
                {
                    Thread.Sleep(100);
                }
            }).Wait(Timeout.InfiniteTimeSpan);
        }
    }
}
