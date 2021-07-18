using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spectre.Console;

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

        public static bool SetEnvironment(string app, string version)
        {
            var selectedEnvs = _envs.Where(e =>
            {
                foreach (var (appName, appVersion) in e.Apps)
                {
                    if (appName != app) continue;
                    if (string.IsNullOrEmpty(appVersion))
                    {
                        return true;
                    }

                    return appVersion == version;
                }
                return false;
            }).ToList();
            if (selectedEnvs.Count == 0)
            {
                return false;
            }

            var envPaths = selectedEnvs.SelectMany(es => es.Paths).ToHashSet();
            var envLdLibraryPaths = selectedEnvs.SelectMany(es => es.LdLibraryPaths).ToHashSet();
            var envAdditionalVariables = selectedEnvs.SelectMany(es => es.AdditionalVariable).ToHashSet(new KeyEqualityComparer());
            
            var table = new Table();
            table.AddColumn("环境变量");
            table.AddColumn("值");
            if (envPaths.Count > 0)
            {
                var valid = ValidatePath(envPaths);
                var envString = string.Join(':', valid);
                System.Environment.SetEnvironmentVariable("PATH", envString + ":" + System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process), EnvironmentVariableTarget.Process);
                table.AddRow("PATH", System.Environment.GetEnvironmentVariable("PATH")!);
            }

            if (envLdLibraryPaths.Count > 0)
            {
                var valid = ValidatePath(envLdLibraryPaths);
                var envString = string.Join(':', valid);
                var exist = System.Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
                if (string.IsNullOrEmpty(exist))
                {
                    System.Environment.SetEnvironmentVariable("LD_LIBRARY_PATH", envString);
                }
                else
                {
                    System.Environment.SetEnvironmentVariable("LD_LIBRARY_PATH", envString + ":" + exist);
                }
                table.AddRow("LD_LIBRARY_PATH", System.Environment.GetEnvironmentVariable("LD_LIBRARY_PATH")!);
            }

            if (envAdditionalVariables.Count > 0)
            {
                foreach (var (key, value) in envAdditionalVariables)
                {
                    System.Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);
                    table.AddRow(key, value);
                }
            }

            table.Expand();
            AnsiConsole.Render(table);
            

            return true;
        }

        private static IEnumerable<string> ValidatePath(IEnumerable<string> origin)
        {
            return origin.Where(p =>
            {
                if (Directory.Exists(p))
                {
                    return true;
                }
                AnsiConsole.MarkupLine($"[red]路径不存在 {p.EscapeMarkup()}[/]");
                return false;
            });
        }

        private class KeyEqualityComparer : IEqualityComparer<KeyValuePair<string, string>>
        {
            public bool Equals(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
            {
                return x.Key == y.Key;
            }

            public int GetHashCode(KeyValuePair<string, string> obj)
            {
                return HashCode.Combine(obj.Key, obj.Value);
            }
        }
    }
}
