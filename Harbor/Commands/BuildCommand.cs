using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Features.Bootstrapping;
using Cake.Features.Building;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Harbor.Commands
{
    public sealed class BuildCommand : Command<BuildCommandSettings>
    {
        private readonly IBuildFeature _builder;
        private readonly IBootstrapFeature _bootstrapper;
        private readonly IConsole _console;
        private readonly ICakeLog _log;

        public BuildCommand(IBuildFeature builder,
            IBootstrapFeature bootstrapper,
            IConsole console,
            ICakeLog log)
        {
            _builder = builder;
            _bootstrapper = bootstrapper;
            _console = console;
            _log = log;
        }

        public override int Execute(CommandContext context, BuildCommandSettings settings)
        {
            try
            {
                // Set log verbosity.
                _log.Verbosity = settings.Verbosity;

                // Get the build host type.
                var host = GetBuildHostKind(settings);

                // Run the bootstrapper?
                if (!settings.SkipBootstrap || settings.Bootstrap)
                {
                    int bootstrapperResult = PerformBootstrapping(context, settings, host);
                    if (bootstrapperResult != 0 || settings.Bootstrap)
                    {
                        return bootstrapperResult;
                    }
                }

                // Run the build feature.
                return _builder.Run(context.Remaining, new BuildFeatureSettings(host)
                {
                    Script = settings.Script,
                    Verbosity = settings.Verbosity,
                    Exclusive = false,
                    Debug = false,
                    NoBootstrapping = settings.SkipBootstrap,
                });
            }
            catch (Exception ex)
            {
                return LogException(_log, ex);
            }
        }

        private static int LogException<T>(ICakeLog log, T ex)
            where T : Exception
        {
            log ??= new CakeBuildLog(
                new CakeConsole(new CakeEnvironment(new CakePlatform(), new CakeRuntime())));

            if (log.Verbosity == Verbosity.Diagnostic)
            {
                //log.Error("Error: {0}", ex);
                AnsiConsole.WriteException(ex,
                    ExceptionFormats.ShortenPaths | ExceptionFormats.ShortenTypes |
                    ExceptionFormats.ShortenMethods | ExceptionFormats.ShowLinks);
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

        private BuildHostKind GetBuildHostKind(BuildCommandSettings settings)
        {
            if (settings.DryRun)
            {
                return BuildHostKind.DryRun;
            }

            return BuildHostKind.Build;
        }

        private int PerformBootstrapping(CommandContext context, BuildCommandSettings settings, BuildHostKind host)
        {
            if (host != BuildHostKind.Build && host != BuildHostKind.DryRun)
            {
                return 0;
            }

            return _bootstrapper.Run(context.Remaining, new BootstrapFeatureSettings
            {
                Script = settings.Script,
                Verbosity = settings.Verbosity
            });
        }
    }
}
