using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;

namespace Harbor.Python
{
    public static class PythonHelper
    {
        public static void SetEnvironment()
        {
#if DEBUG
            Runtime.PythonDLL = @"C:\Python\Python38\python38.dll";
            PythonEngine.PythonHome = @"C:\Python\Python38";
            var pathToVirtualEnv = @"C:\Python\Python38";
            Environment.SetEnvironmentVariable("PATH", @"C:\Python\Python38;E:\Tools\iverilog\bin", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToVirtualEnv, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH", $"{pathToVirtualEnv}\\Lib\\site-packages;{pathToVirtualEnv}\\Lib", EnvironmentVariableTarget.Process);
#else
            Runtime.PythonDLL = @"C:\Python\Python38\python38.dll";
            PythonEngine.PythonHome = @"/export/yfxie02/bin/python/";
            var pathToVirtualEnv = @"/export/yfxie02/bin/python/";
            Environment.SetEnvironmentVariable("PATH", @"/export/yfxie02/bin/python/bin;E:\Tools\iverilog\bin", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToVirtualEnv, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH", $"{pathToVirtualEnv}\\Lib\\site-packages;{pathToVirtualEnv}\\Lib", EnvironmentVariableTarget.Process);
#endif

            PythonEngine.PythonPath = PythonEngine.PythonPath + ";" + Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);
        }
    }
}
