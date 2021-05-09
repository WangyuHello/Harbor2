using System.ComponentModel;
using Harbor.Common.Project;
using Spectre.Console.Cli;

namespace Harbor.Commands
{
    public class InitCommandSettings: CommandSettings
    {
        [CommandArgument(0, "[NAME]")]
        public string Name { get; set; }

        [CommandOption("-t|--type <TYPE>")]
        [TypeConverter(typeof(ProjectTypeConverter))]
        [DefaultValue(null)]
        public ProjectType? Type { get; set; }

        [CommandOption("-l|--library <LIBRARY>")]
        [TypeConverter(typeof(LibraryConverter))]
        [DefaultValue(null)]
        public Library Library { get; set; }

        [CommandOption("-s|--stdcell <STDCELL>")]
        public string StdCell { get; set; }

        [CommandOption("--io <IO>")]
        // ReSharper disable once InconsistentNaming
        public string IO { get; set; }

        [CommandOption("-c|--clock <CLOCK>")]
        public string ClockName { get; set; }

        [CommandOption("--cp|--clockperiod <CLOCKPERIOD>")]
        public double? ClockPeriod { get; set; }

        [CommandOption("-r|--reset <RESET>")]
        public string Reset { get; set; }
    }
}
