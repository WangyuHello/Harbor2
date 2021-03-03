using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;
using Harbor.Cli;

namespace Harbor.Commands
{
    public sealed class DefaultCommand : Command<DefaultCommandSettings>
    {
        private readonly IHarborVersionFeature _version;
        private readonly IHarborInfoFeature _info;
        private readonly IConsole _console;
        private readonly ICakeLog _log;

        public DefaultCommand(
            IHarborVersionFeature version,
            IHarborInfoFeature info,
            IConsole console,
            ICakeLog log)
        {
            _version = version;
            _info = info;
            _console = console;
            _log = log;
        }

        public override int Execute(CommandContext context, DefaultCommandSettings settings)
        {
            try
            {
                // Set log verbosity.
                _log.Verbosity = settings.Verbosity;

                if (settings.ShowVersion)
                {
                    _version.Run(_console);
                    return 0;
                }
                else if (settings.ShowInfo)
                {
                    _info.Run(_console);
                    return 0;
                }

                return 0;
            }
            catch (Exception ex)
            {
                return LogException(_log, ex);
            }
        }

        private static int LogException<T>(ICakeLog log, T ex)
            where T : Exception
        {
            log = log ?? new CakeBuildLog(
                      new CakeConsole(new CakeEnvironment(new CakePlatform(), new CakeRuntime())));

            if (log.Verbosity == Verbosity.Diagnostic)
            {
                log.Error("Error: {0}", ex);
            }
            else
            {
                log.Error("Error: {0}", ex.Message);
                if (ex is AggregateException aex)
                {
                    foreach (var exception in aex.Flatten().InnerExceptions)
                    {
                        log.Error("\t{0}", exception.Message);
                    }
                }
            }

            return 1;
        }
    }
}
