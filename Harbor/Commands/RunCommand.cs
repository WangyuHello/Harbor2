using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Cake.Core.Diagnostics;
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
            EnvironmentHelper.SetEnvironment(app);
            return await SimpleRunner.Run(app, "", redirectOutput: true);
        }

        private void WaitForDebugger()
        {
            var processId = Process.GetCurrentProcess().Id;
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
