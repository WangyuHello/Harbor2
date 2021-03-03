using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harbor.Commands.Project;
using Harbor.Commands.Template;
using Harbor.Commands.Util;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Harbor.Commands
{
    public sealed class InitCommand : AsyncCommand<InitCommandSettings>
    {
        private static ProjectType ReadProjectType()
        {
            ReadProjectTypeBegin:
            var table = new Table();
            table.AddColumn("选项");
            table.AddColumn("名称");
            table.AddColumn("备注");
            table.AddRow("[red]analog[/]", "模拟工程", "Cadence Virtuoso | Spectre | AMS");
            table.AddRow("[red]digital[/]", "数字工程", "Synopsys DC | ICC | ModelSim");
            table.AddRow("[red]memory[/]", "存储器工程", "Memory Compiler");
            table.AddRow("[red]ip[/]", "IP工程", "PLL | Serdes 等");
            AnsiConsole.Render(table);

            var typeString = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]请选择项目类型[/]")
                    .PageSize(5)
                    .AddChoices("analog", "digital", "memory", "ip"));

            var result = ProjectTypeConverter.Lookup.TryGetValue(typeString, out var type);
            if (!result)
            {
                goto ReadProjectTypeBegin;
            }
            return type;
        }

        private static Library ReadLibrary(List<Library> libraries)
        {
            var table = new Table();
            table.AddColumn("名称");
            table.AddColumn("备注");

            foreach (var lib in libraries)
            {
                table.AddRow("[red]" + lib.Name + "[/]", "");
            }

            AnsiConsole.Render(table);

            var libString = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]请选择工艺库[/]")
                    .PageSize(10)
                    .AddChoices(libraries.Select(lib => lib.Name)));

            return AllLibrary.GetLibrary(libString);
        }

        private static List<Library.LibraryStdCell> ReadStdCell(Library libr)
        {
            var table = new Table();
            table.AddColumn("名称");
            table.AddColumn("备注");

            foreach (var lib in libr.StdCell)
            {
                table.AddRow("[red]" + lib.Name + "[/]", lib.description ?? "");
            }

            AnsiConsole.Render(table);

            var libString = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("[blue]请选择标准单元库[/]")
                    .NotRequired()
                    .PageSize(10)
                    //.AddChoice("<不选择>")
                    .AddChoices(libr.StdCell.Select(lib => lib.Name)));

            if (libString != null && libString.Count != 0)
            {
                return libString.Select(libr.GetStdCell).ToList();
            }

            return null;
        }

        private static List<Library.LibraryIo> ReadIo(Library libr)
        {
            var table = new Table();
            table.AddColumn("名称");
            table.AddColumn("备注");

            foreach (var lib in libr.Io)
            {
                table.AddRow("[red]" + lib.Name + "[/]", "");
            }

            AnsiConsole.Render(table);

            var libString = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("[blue]请选择标准单元库[/]")
                    .NotRequired()
                    .PageSize(10)
                    //.AddChoice("<不选择>")
                    .AddChoices(libr.Io.Select(lib => lib.Name)));

            if (libString != null && libString.Count != 0)
            {
                return libString.Select(libr.GetIo).ToList();
            }

            return null;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, InitCommandSettings settings)
        {
            var projectInfo = new ProjectInfo();

            if (settings.Type is null)
            {
                settings.Type = ReadProjectType();
                projectInfo.Type = (ProjectType)settings.Type;
            }

            if (settings.Library is null)
            {
                settings.Library = ReadLibrary(AllLibrary.Libraries);
                projectInfo.Library = settings.Library.Name;
            }

            List<Library.LibraryStdCell> stdCell = null;
            List<Library.LibraryIo> io = null;

            if (settings.Type == ProjectType.Analog || settings.Type == ProjectType.Digital)
            {
                if (settings.Library.HasStdCell)
                {
                    var stdC = settings.Library.GetStdCell(settings.StdCell);
                    if (stdC is null)
                    {
                        stdCell =  ReadStdCell(settings.Library);
                    }
                    else
                    {
                        stdCell = new List<Library.LibraryStdCell>()
                        {
                            stdC
                        };
                    }
                }

                if (settings.Library.HasIo)
                {
                    var ioC = settings.Library.GetIo(settings.IO);
                    if (ioC is null)
                    {
                        io = ReadIo(settings.Library);
                    }
                    else
                    {
                        io = new List<Library.LibraryIo>()
                        {
                            ioC
                        };
                    }
                }
            }

            if (stdCell != null && stdCell.Count != 0)
            {
                projectInfo.StdCell = stdCell.Select(s => s.Name).ToList();
            }

            if (io != null && io.Count != 0)
            {
                projectInfo.Io = io.Select(s => s.Name).ToList();
            }

            switch (settings.Type)
            {
                case ProjectType.Analog:
                    if (string.IsNullOrEmpty(settings.Name))
                    {
                        settings.Name = AnsiConsole.Ask<string>("请输入项目名称");
                        projectInfo.Project = settings.Name;
                    }

                    if (Directory.Exists(settings.Name))
                    {
                        var overr = AnsiConsole.Confirm($"{settings.Name}项目已存在是否覆盖?");
                        if (overr)
                        {
                            Directory.Delete(Path.Combine(Environment.CurrentDirectory, settings.Name), true);
                        }
                        else
                        {
                            return 0;
                        }
                    }

                    var projectDir = Path.Combine(Environment.CurrentDirectory, settings.Name);

                    Directory.CreateDirectory(projectDir);
                    Directory.CreateDirectory(Path.Combine(projectDir, ".harbor"));
                    Directory.CreateDirectory(Path.Combine(projectDir, "Validation"));
                    Directory.CreateDirectory(Path.Combine(projectDir, "Validation", "DRC"));
                    Directory.CreateDirectory(Path.Combine(projectDir, "Validation", "LVS"));
                    Directory.CreateDirectory(Path.Combine(projectDir, "Validation", "PEX"));
                    
                    await CdsUtil.CreateCdsLibAsync(projectDir, settings.Library.Pdk, stdCell, io);
                    
                    break;
                case ProjectType.Digital:
                    if (string.IsNullOrEmpty(settings.Name))
                    {
                        settings.Name = AnsiConsole.Ask<string>("请输入顶层模块名称");
                        projectInfo.Project = settings.Name;
                    }

                    if (Directory.Exists(settings.Name))
                    {
                        var overr = AnsiConsole.Confirm($"{settings.Name} 项目已存在是否覆盖?");
                        if (overr)
                        {
                            Directory.Delete(Path.Combine(Environment.CurrentDirectory, settings.Name), true);
                        }
                        else
                        {
                            return 0;
                        }
                    }

                    if (string.IsNullOrEmpty(settings.ClockName))
                    {
                        settings.ClockName = AnsiConsole.Prompt(
                            new TextPrompt<string>("[grey][[可选]][/] 请输入时钟名称")
                                .AllowEmpty());
                        if (string.IsNullOrEmpty(settings.ClockName))
                        {
                            settings.ClockName = "vclk";
                        }
                    }

                    if (!string.IsNullOrEmpty(settings.ClockName))
                    {
                        if (string.IsNullOrEmpty(settings.Reset))
                        {
                            settings.Reset = AnsiConsole.Prompt(
                                new TextPrompt<string>("[grey][[可选]][/] 请输入异步复位名称")
                                    .AllowEmpty());
                        }

                        if (settings.ClockPeriod is null)
                        {
                            settings.ClockPeriod = AnsiConsole.Prompt(
                               new TextPrompt<double?>("[grey][[可选]][/] 请输入时钟周期(ns)")
                                   .AllowEmpty()) ?? 10;
                        }
                    }
                    else
                    {
                        if (settings.ClockPeriod is null)
                        {
                            settings.ClockPeriod = AnsiConsole.Prompt(
                                new TextPrompt<double?>("[grey][[可选]][/] 请输入虚拟时钟周期(ns)")
                                    .AllowEmpty()) ?? 10;
                        }
                    }

                    var projectDir2 = Path.Combine(Environment.CurrentDirectory, settings.Name);

                    Directory.CreateDirectory(projectDir2);
                    Directory.CreateDirectory(Path.Combine(projectDir2, ".harbor"));
                    Directory.CreateDirectory(Path.Combine(projectDir2, "Source"));
                    Directory.CreateDirectory(Path.Combine(projectDir2, "SimulationSource"));
                    Directory.CreateDirectory(Path.Combine(projectDir2, "Simulation"));
                    Directory.CreateDirectory(Path.Combine(projectDir2, "Simulation", "Behavior"));
                    Directory.CreateDirectory(Path.Combine(projectDir2, "Simulation", "Layout"));
                    Directory.CreateDirectory(Path.Combine(projectDir2, "Simulation", "Synthesis"));
                    Directory.CreateDirectory(Path.Combine(projectDir2, "Cadence"));

                    var buildCake = new BuildCake
                    {
                        ClockName = settings.ClockName,
                        ClockPeriod = settings.ClockPeriod,
                        ResetName = settings.Reset
                    };

                    var text = buildCake.TransformText();
                    await File.WriteAllTextAsync(Path.Combine(projectDir2, "build.hb"), text.Replace("\r", ""), encoding: Encoding.UTF8);

                    break;
            }

            return 0;
        }
    }
}
