using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;

namespace Harbor.Python
{
    public static class PythonHelper
    {
        private static string _banner;

        public static string Banner
        {
            get
            {
                if (!string.IsNullOrEmpty(_banner)) return _banner;
                var assembly = Assembly.GetEntryAssembly();
                var version = FileVersionInfo.GetVersionInfo(assembly!.Location).Comments;

                _banner = "// Created by: Harbor" + Environment.NewLine +
                          "// Version   : " + version + Environment.NewLine +
                          "// Time      : ";

                return _banner;
            }
        }

        public static void SetEnvironment(string workingDirectory, Action code)
        {
            var curDir = Environment.CurrentDirectory;
            Environment.CurrentDirectory = workingDirectory;

            if (PythonEngine.IsInitialized)
            {
                PythonEngine.Shutdown();
            }
#if DEBUG
            var pathToVirtualEnv = @"C:\Python\Python38";
            Runtime.PythonDLL = $"{pathToVirtualEnv}\\python38.dll";
            PythonEngine.PythonHome = pathToVirtualEnv;
            Environment.SetEnvironmentVariable("PATH", $"{pathToVirtualEnv};E:\\Tools\\iverilog\\bin", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToVirtualEnv, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH", $"{pathToVirtualEnv}\\Lib\\site-packages;{pathToVirtualEnv}\\Lib", EnvironmentVariableTarget.Process);
            PythonEngine.PythonPath = PythonEngine.PythonPath + ";" + Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);
#else
            var pathToVirtualEnv = "/export/yfxie02/bin/python";
            Runtime.PythonDLL = $"{pathToVirtualEnv}/lib/libpython3.8.so.1.0";
            PythonEngine.PythonHome = pathToVirtualEnv;
            Environment.SetEnvironmentVariable("PATH", $"{pathToVirtualEnv}/bin:" + Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process), EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToVirtualEnv, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH", $"{pathToVirtualEnv}/lib/python3.8/site-packages:{pathToVirtualEnv}/lib", EnvironmentVariableTarget.Process);
            PythonEngine.PythonPath = PythonEngine.PythonPath + ":" + Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);
#endif
            using (Py.GIL())
            {
                code?.Invoke();
            }

            Environment.CurrentDirectory = curDir;
        }

        public static string GetCodeFromResource(string fullName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullName);
            using var sr = new StreamReader(stream ?? throw new InvalidOperationException(), new UTF8Encoding(false));
            var code = sr.ReadToEnd();
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                code = code.Replace("\r","");
            }
            return code;
        }
    }
}
