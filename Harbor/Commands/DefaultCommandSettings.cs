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
        [Description("设置日志等级\n(Quiet, Minimal, Normal, Verbose, Diagnostic)")]
        [TypeConverter(typeof(VerbosityConverter))]
        [DefaultValue(Verbosity.Normal)]
        public Verbosity Verbosity { get; set; }

        [CommandOption("--version|--ver")]
        [Description("显示版本")]
        public bool ShowVersion { get; set; }

        [CommandOption("--info")]
        [Description("显示更多信息")]
        public bool ShowInfo { get; set; }
    }
}
