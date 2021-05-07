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
        [CommandOption("--verbosity|-v <VERBOSITY>")]
        [Description("Specifies the amount of information to be displayed.\n(Quiet, Minimal, Normal, Verbose, Diagnostic)")]
        [TypeConverter(typeof(VerbosityConverter))]
        [DefaultValue(Verbosity.Normal)]
        public Verbosity Verbosity { get; set; }

        [CommandOption("--version|--ver")]
        [Description("Displays version information.")]
        public bool ShowVersion { get; set; }

        [CommandOption("--info")]
        [Description("Displays additional information about Cake.")]
        public bool ShowInfo { get; set; }
    }
}
