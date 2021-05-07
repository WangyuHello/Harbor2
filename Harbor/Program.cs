using Autofac;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Spectre.Console.Cli;
using System;
using System.Threading.Tasks;
using Cake.Features.Bootstrapping;
using Cake.Features.Building;
using Cake.Infrastructure;
using Cake.Infrastructure.Composition;
using Harbor.Commands;
using Harbor.Cli;

namespace Harbor
{
    public sealed class Program
    {
        private readonly Action<ContainerBuilder> _overrides;
        private readonly bool _propagateExceptions;

        public Program(
            Action<ContainerBuilder> overrides = null,
            bool propagateExceptions = false)
        {
            _overrides = overrides;
            _propagateExceptions = propagateExceptions;
        }

        public static async Task<int> Main(string[] args)
        {
            return await new Program().Run(args);
        }

        public async Task<int> Run(string[] args)
        {
            var registrar = BuildTypeRegistrar();

            var app = new CommandApp<DefaultCommand>(registrar);

            app.Configure(config =>
            {
                config.AddCommand<InitCommand>("init");
                config.AddCommand<AddRefCommand>("addref");
                config.AddCommand<BuildCommand>("build");

                config.SetApplicationName("harhor");
                config.ValidateExamples();

                if (_propagateExceptions)
                {
                    config.PropagateExceptions();
                }

                // Top level examples.
                config.AddExample(new[] { string.Empty });
            });
            return await app.RunAsync(args);
        }

        private ITypeRegistrar BuildTypeRegistrar()
        {
            var builder = new ContainerBuilder();

            // Converters
            builder.RegisterType<Cake.Cli.FilePathConverter>();
            builder.RegisterType<Cake.Cli.VerbosityConverter>();

            // Utilities
            builder.RegisterType<ContainerConfigurator>().As<IContainerConfigurator>().SingleInstance();
            builder.RegisterType<VersionResolver>().As<IVersionResolver>().SingleInstance();
            builder.RegisterType<ModuleSearcher>().As<IModuleSearcher>().SingleInstance();

            // Features
            builder.RegisterType<BuildFeature>().As<IBuildFeature>().SingleInstance();
            builder.RegisterType<BootstrapFeature>().As<IBootstrapFeature>().SingleInstance();
            builder.RegisterType<VersionFeature>().As<IHarborVersionFeature>().SingleInstance();
            builder.RegisterType<InfoFeature>().As<IHarborInfoFeature>().SingleInstance();

            // Core
            builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            builder.RegisterType<CakeEnvironment>().As<ICakeEnvironment>().SingleInstance();
            builder.RegisterType<CakePlatform>().As<ICakePlatform>().SingleInstance();
            builder.RegisterType<CakeRuntime>().As<ICakeRuntime>().SingleInstance();
            builder.RegisterType<CakeBuildLog>().As<ICakeLog>().SingleInstance();
            builder.RegisterType<CakeConsole>().As<IConsole>().SingleInstance();

            // Register custom registrations.
            _overrides?.Invoke(builder);

            return new AutofacTypeRegistrar(builder);
        }
    }
}
