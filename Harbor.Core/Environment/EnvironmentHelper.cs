using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spectre.Console;
// ReSharper disable IdentifierTypo

namespace Harbor.Core.Environment
{
    public static class EnvironmentHelper
    {
        private static readonly List<(IEnvironmentDefinition env, bool isDefault)> _envs;

        static EnvironmentHelper()
        {
            _envs = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IEnvironmentDefinition))))
                .Select(t =>
                {
                    var c = Activator.CreateInstance(t) as IEnvironmentDefinition;
                    var attrs = Attribute.GetCustomAttributes(t);
                    var isDefault = attrs.Select(a => a is DefaultEnvironmentAttribute).SingleOrDefault();
                    return (c, isDefault);
                }).ToList();
        }


        public static bool SetEnvironment(string app, string version = "")
        {
            var selectedEnvs = GetSelectedEnvs(new [] {app}, version);

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

        private static List<IEnvironmentDefinition> GetSelectedEnvs(IEnumerable<string> app, string version)
        {
            var apps = app as string[] ?? app.ToArray();
            var selectedEnvs = _envs.Where(e =>
            {
                foreach (var (appName, appVersion) in e.env.Apps)
                {
                    if (!apps.Contains(appName)) continue;
                    if (string.IsNullOrEmpty(appVersion))
                    {
                        return true;
                    }

                    if (string.IsNullOrEmpty(version))
                    {
                        return e.isDefault;
                    }

                    return appVersion == version;
                }

                return false;
            }).Select(tup => tup.env).ToList();
            return selectedEnvs;
        }

        public static IDictionary<string, string> AddEnvironment(IEnumerable<string> app, IDictionary<string, string> origin, string version = "")
        {
            var selectedEnvs = GetSelectedEnvs(app, version);

            if (selectedEnvs.Count == 0)
            {
                return origin;
            }

            var envPaths = selectedEnvs.SelectMany(es => es.Paths).ToHashSet();
            var envLdLibraryPaths = selectedEnvs.SelectMany(es => es.LdLibraryPaths).ToHashSet();
            var envAdditionalVariables = selectedEnvs.SelectMany(es => es.AdditionalVariable).ToHashSet(new KeyEqualityComparer());

            if (envPaths.Count > 0)
            {
                var valid = ValidatePath(envPaths);
                var envString = string.Join(':', valid);
                if (origin.ContainsKey("PATH"))
                {
                    origin["PATH"] = envString + ":" + origin["PATH"];
                }
                else
                {
                    origin.Add("PATH", envString);
                }
                System.Environment.SetEnvironmentVariable("PATH", envString + ":" + System.Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process), EnvironmentVariableTarget.Process);
            }

            if (envLdLibraryPaths.Count > 0)
            {
                var valid = ValidatePath(envLdLibraryPaths);
                var envString = string.Join(':', valid);
                if (!origin.ContainsKey("LD_LIBRARY_PATH"))
                {
                    origin.Add("LD_LIBRARY_PATH", envString);
                }
                else
                {
                    origin["LD_LIBRARY_PATH"] = envString + ":" + origin["LD_LIBRARY_PATH"];
                }
            }

            if (envAdditionalVariables.Count > 0)
            {
                foreach (var (key, value) in envAdditionalVariables)
                {
                    origin.Add(key,value);
                }
            }

            return origin;
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
