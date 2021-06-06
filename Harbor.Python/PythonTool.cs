using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.Tooling;
using Python.Runtime;
using Spectre.Console;

namespace Harbor.Python
{
    public abstract class PythonTool<TSettings, TResult> where TSettings : ToolSettings
    {
        protected abstract TResult RunCore(TSettings settings);
        protected string Code;
        public TResult Run(TSettings settings)
        {
            TResult r = default(TResult);
            var className = GetType().FullName;
            //className = MethodBase.GetCurrentMethod()?.DeclaringType?.FullName;
            Code = GetCodeFromResource($"{className}.py");
            SetEnvironment(settings.WorkingDirectory.FullPath, () =>
            {
                PrintBanner(settings);
                r = RunCore(settings);
            });
            return r;
        }

        private string _banner;

        protected string Banner
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

        protected void PrintBanner(TSettings settings)
        {
            var toolName = GetType().FullName?.Split(".")[^1];

            AnsiConsole.Render(new Rule($"Python - {toolName}") { Alignment = Justify.Center, Style = Style.Parse("blue dim") });
            var table = new Table();
            table.AddColumn("运行目录");
            table.AddColumn(new TableColumn(settings.WorkingDirectory.ToString()));
            table.AddRow("版本", PythonEngine.Version.Replace("[", "[[").Replace("]", "]]"));
            table.Expand();
            AnsiConsole.Render(table);
            AnsiConsole.Render(new Rule { Style = Style.Parse("blue dim") });
        }

        private void SetForWindows()
        {
            var pathToVirtualEnv = @"C:\Python\Python38";
            Runtime.PythonDLL = $"{pathToVirtualEnv}\\python38.dll";
            PythonEngine.PythonHome = pathToVirtualEnv;
            Environment.SetEnvironmentVariable("PATH", $"{pathToVirtualEnv};E:\\Tools\\iverilog\\bin", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToVirtualEnv, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH", $"{pathToVirtualEnv}\\Lib\\site-packages;{pathToVirtualEnv}\\Lib", EnvironmentVariableTarget.Process);
            PythonEngine.PythonPath = PythonEngine.PythonPath + ";" + Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);
        }

        public void SetForLinuxServer()
        {
            var pathToVirtualEnv = "/export/yfxie02/bin/python";
            Runtime.PythonDLL = $"{pathToVirtualEnv}/lib/libpython3.8.so.1.0";
            PythonEngine.PythonHome = pathToVirtualEnv;
            Environment.SetEnvironmentVariable("PATH", $"{pathToVirtualEnv}/bin:" + Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process), EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToVirtualEnv, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH", $"{pathToVirtualEnv}/lib/python3.8/site-packages:{pathToVirtualEnv}/lib", EnvironmentVariableTarget.Process);
            PythonEngine.PythonPath = PythonEngine.PythonPath + ":" + Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);
        }

        private void SetEnvironment(string workingDirectory, Action code)
        {
            var curDir = Environment.CurrentDirectory;
            Environment.CurrentDirectory = workingDirectory;

            if (PythonEngine.IsInitialized)
            {
                PythonEngine.Shutdown();
            }
            else
            {   //初次运行
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    SetForLinuxServer();
                }
                else
                {
                    SetForWindows();
                }
            }

            using (Py.GIL())
            {
                code?.Invoke();
            }

            Environment.CurrentDirectory = curDir;
        }

        private string GetCodeFromResource(string fullName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullName);
            using var sr = new StreamReader(stream ?? throw new InvalidOperationException(), new UTF8Encoding(false));
            var code = sr.ReadToEnd();
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                code = code.Replace("\r", "");
            }
            return code;
        }
    }

    public abstract class PythonTool<TSettings> : PythonTool<TSettings, int> where TSettings : ToolSettings
    {
 
    }
}
