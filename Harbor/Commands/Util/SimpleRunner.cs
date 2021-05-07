using System.Diagnostics;
using System.Threading.Tasks;

namespace Harbor.Commands.Util
{
    public static class SimpleRunner
    {
        public static async Task<int> Run(string filename, string arguments, string workingDirectory = "")
        {
            var psi = new ProcessStartInfo(filename, arguments)
            {
                CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden, UseShellExecute = true
            };

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                psi.WorkingDirectory = workingDirectory;
            }

            var p = Process.Start(psi);
            if (p == null) return -1;
            await p.WaitForExitAsync();
            return p.ExitCode;
        }
    }
}
