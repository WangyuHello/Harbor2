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

            var p = Process.Start(psi);
            if (p == null) return -1;

            if (redirectOutput)
            {
                p.OutputDataReceived += (sender, args) => AnsiConsole.MarkupLine(args.Data ?? string.Empty);
                p.ErrorDataReceived += (sender, args) => AnsiConsole.MarkupLine(args.Data ?? string.Empty);
            }

            await p.WaitForExitAsync();
            return p.ExitCode;
        }
    }
}
