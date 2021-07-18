using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Harbor.Commands.Util;
using Harbor.Common.Project;
using Harbor.Core.Util;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Harbor.Commands
{
    public sealed class AddRefCommand : AsyncCommand<AddRefCommandSettings>
    {
        private static bool IsDirectory(FileInfo fi)
        {
            return (fi.Attributes & FileAttributes.Directory) != 0;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, AddRefCommandSettings settings)
        {
            var info = await ProjectInfo.ReadFromCurrentDirectoryAsync();

            if (string.IsNullOrEmpty(settings.Reference))
            {
                AnsiConsole.MarkupLine("[red]引用项目不能为空[/]");
                return -1;
            }

            if (Directory.Exists(settings.Reference))
            {
                //是文件夹
                var di = new DirectoryInfo(settings.Reference);
                settings.Reference = di.FullName;
            }
            else if(File.Exists(settings.Reference))
            {
                //是文件
                FileInfo fi = new(settings.Reference);
                settings.Reference = fi.DirectoryName;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]引用项目不存在[/]");
                return -1;
            }

            var refInfo = await ProjectInfo.ReadFromDirectoryAsync(settings.Reference);
            var newPjInfo = info;

            switch (info.Type)
            {
                case ProjectType.Analog:
                    switch (refInfo.Type)
                    {
                        case ProjectType.Digital:
                            AnsiConsole.MarkupLine($"[underline green]添加数字工程引用[/] {settings.Reference}");
                            newPjInfo = AddRefToProjectInfo(info, refInfo, settings.Reference);
                            break;
                        default:
                            AnsiConsole.MarkupLine("[red]暂不支持[/]");
                            break;
                    }
                    AnsiConsole.MarkupLine("[underline yellow]刷新cds.lib[/]");
                    await CdsUtil.RefreshCdsLibAsync(System.Environment.CurrentDirectory, newPjInfo);
                    break;
                case ProjectType.Digital:
                    switch (refInfo.Type)
                    {
                        case ProjectType.Memory:
                        case ProjectType.Ip:
                            newPjInfo = AddRefToProjectInfo(info, refInfo, settings.Reference);
                            AnsiConsole.MarkupLine($"[underline green]添加引用[/] {settings.Reference}");
                            break;
                        default:
                            AnsiConsole.MarkupLine("[red]暂不支持[/]");
                            break;
                    }
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]暂不支持[/]");
                    break;
            }

            await newPjInfo.WriteToCurrentDirectoryAsync();
            return 0;
        }

        private static ProjectInfo AddRefToProjectInfo(ProjectInfo projectInfo, ProjectInfo refProjectInfo, string reference)
        {
            if (projectInfo.Reference != null)
            {
                var refPjName = refProjectInfo.Project;

                if (projectInfo.Reference.FirstOrDefault(r => r.Name == refPjName) != null)
                {
                    AnsiConsole.MarkupLine("[red]已经添加过同名的项目[/]");
                    return projectInfo;
                }

                if (projectInfo.Library != refProjectInfo.Library)
                {
                    var confirm = AnsiConsole.Confirm("引用项目的工艺库与当前项目冲突，是否继续添加引用?");
                    if (!confirm)
                    {
                        return projectInfo;
                    }
                }

                projectInfo.Reference.Add(new ProjectReference
                {
                    Name = refPjName,
                    Path = reference.GetRelativePath()
                });

                return projectInfo;
            }

            projectInfo.Reference = new List<ProjectReference>
            {
                new()
                {
                    Name = refProjectInfo.Project,
                    Path = reference.GetRelativePath()
                }
            };
            return projectInfo;
        }
    }
}
