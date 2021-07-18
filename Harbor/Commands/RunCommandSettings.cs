using System.ComponentModel;
using Spectre.Console.Cli;

namespace Harbor.Commands
{
    public class RunCommandSettings : CommandSettings
    {
        [CommandArgument(0, "[EDA程序]")]
        public string App { get; set; }

        [CommandOption("-v|--version <版本>")]
        [DefaultValue("")]
        public string Version { get; set; }

        [CommandOption("--debug|-d")]
        [Description("Launches script in debug mode.")]
        public bool Debug { get; set; }
    }
}
