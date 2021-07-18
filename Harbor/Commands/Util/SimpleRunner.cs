using System.Diagnostics;
using System.Threading.Tasks;
using Spectre.Console;

namespace Harbor.Commands.Util
{
    public static class SimpleRunner
    {
        public static async Task<int> Run(string filename, string arguments, string workingDirectory = "", bool redirectOutput = false)
        {
            var psi = new ProcessStartInfo(filename, arguments)
            {
                CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden, UseShellExecute = true
            };

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                psi.WorkingDirectory = workingDirectory;
            }

            if (redirectOutput)
            {
                psi.UseShellExecute = false;
                psi.RedirectStandardError = true;
                psi.RedirectStandardOutput = true;
            }

            var p = new Process
            {
                StartInfo = psi
            };

            if (redirectOutput)
            {
                p.OutputDataReceived += (_, args) => AnsiConsole.MarkupLine(args.Data?.EscapeMarkup() ?? string.Empty);
                p.ErrorDataReceived += (_, args) => AnsiConsole.MarkupLine($"[red]{args.Data?.EscapeMarkup() ?? string.Empty}[/]");
            }

            p.Start();
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();

            await p.WaitForExitAsync();
            return p.ExitCode;
        }
    }
}
