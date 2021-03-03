using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Harbor.Commands
{
    public sealed class AddRefCommand : Command<AddRefCommandSettings>
    {
        public override int Execute(CommandContext context, AddRefCommandSettings settings)
        {
            AnsiConsole.Markup($"[underline red]添加引用[/] {settings.Reference}!");
            return 0;
        }
    }
}
