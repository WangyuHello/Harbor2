using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Cli;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Spectre.Console.Cli;

namespace Harbor.Commands
{
    public sealed class DefaultCommandSettings : CommandSettings
    {
        [CommandArgument(0, "[SCRIPT]")]
        [Description("The Cake script. Defaults to [grey]build.hb[/]")]
        [TypeConverter(typeof(FilePathConverter))]
        [DefaultValue("build.hb")]
        public FilePath Script { get; set; }

        [CommandOption("--bootstrap")]
        [Description("Download/install modules defined by [grey]#module[/] directives, but do not run build.")]
        public bool Bootstrap { get; set; }

        [CommandOption("--skip-bootstrap")]
        [Description("Skips bootstrapping when running build.")]
        public bool SkipBootstrap { get; set; }

        [CommandOption("--debug|-d")]
        [Description("Launches script in debug mode.")]
        public bool Debug { get; set; }

        [CommandOption("--verbosity|-v <VERBOSITY>")]
        [Description("Specifies the amount of information to be displayed.\n(Quiet, Minimal, Normal, Verbose, Diagnostic)")]
        [TypeConverter(typeof(VerbosityConverter))]
        [DefaultValue(Verbosity.Normal)]
        public Verbosity Verbosity { get; set; }

        [CommandOption("--description|--descriptions|--showdescription|--showdescriptions")]
        [Description("Shows description for each task.")]
        public bool Description { get; set; }

        [CommandOption("--tree|--showtree")]
        [Description("Shows the task dependency tree.")]
        public bool Tree { get; set; }

        [CommandOption("--dryrun|--noop|--whatif")]
        [Description("Performs a dry run.")]
        public bool DryRun { get; set; }

        [CommandOption("--exclusive|-e")]
        [Description("Executes the target task without any dependencies.")]
        public bool Exclusive { get; set; }

        [CommandOption("--version|--ver")]
        [Description("Displays version information.")]
        public bool ShowVersion { get; set; }

        [CommandOption("--info")]
        [Description("Displays additional information about Cake.")]
        public bool ShowInfo { get; set; }
    }
}
