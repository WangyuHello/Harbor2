using System;
using System.Collections.Generic;
using System.Linq;

namespace Harbor.Commands.Environment
{
    public static class EnvironmentHelper
    {
        private static readonly List<IEnvironmentDefinition> _envs;

        static EnvironmentHelper()
        {
            _envs = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IEnvironmentDefinition))))
                .Select(t => Activator.CreateInstance(t) as IEnvironmentDefinition).ToList();
        }

        public static void SetEnvironment(string app)
        {
            var selectedEnvs = _envs.Where(e => e.Apps.Contains(app));
            foreach (var env in selectedEnvs)
            {
                if (env.Paths != null)
                {
                    var envString = string.Join(':', env.Paths);
                    System.Environment.SetEnvironmentVariable("PATH", envString + ":" + System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process), EnvironmentVariableTarget.Process);
                }

                if (env.LdLibraryPaths != null)
                {
                    var envString = string.Join(':', env.LdLibraryPaths);
                    var exist = System.Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
                    if (string.IsNullOrEmpty(exist))
                    {
                        System.Environment.SetEnvironmentVariable("LD_LIBRARY_PATH", envString);
                    }
                    else
                    {
                        System.Environment.SetEnvironmentVariable("LD_LIBRARY_PATH", envString + ":" + exist);
                    }
                }

                if (env.AdditionalVariable != null)
                {
                    foreach (var (key, value) in env.AdditionalVariable)
                    {
                        System.Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);
                    }
                }
            }
        }
    }
}
