using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console.Cli;

namespace Harbor.Commands
{
    public sealed class AddRefCommandSettings : CommandSettings
    {
        [CommandArgument(0, "[引用项目]")]
        public string Reference { get; set; }
    }
}
