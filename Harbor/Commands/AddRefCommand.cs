using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Harbor.Core.Project;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Harbor.Commands
{
    public sealed class AddRefCommand : AsyncCommand<AddRefCommandSettings>
    {
        private static bool IsDirectory(FileInfo fi)
        {
            if ((fi.Attributes & FileAttributes.Directory) != 0)
                return true;
            return false;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, AddRefCommandSettings settings)
        {
            AnsiConsole.Markup($"[underline red]添加引用[/] {settings.Reference}!");
            
            var info = await ProjectInfo.ReadFromCurrentDirectoryAsync();

            if (string.IsNullOrEmpty(settings.Reference))
            {
                AnsiConsole.Markup("[red]引用项目不能为空[/]");
                return -1;
            }

            FileInfo fi = new FileInfo(settings.Reference);
            if (!fi.Exists)
            {
                AnsiConsole.Markup("[red]引用项目不存在[/]");
                return -1;
            }

            if (!IsDirectory(fi))
            {
                settings.Reference = fi.DirectoryName;
            }

            var refInfo = await ProjectInfo.ReadFromDirectoryAsync(settings.Reference);
            var newPjInfo = info;

            switch (info.Type)
            {
                case ProjectType.Analog:
                    switch (refInfo.Type)
                    {
                        case ProjectType.Digital:
                            newPjInfo = AddRefToProjectInfo(info, refInfo, settings.Reference);
                            //Refresh Cds
                            break;
                        default:
                            AnsiConsole.Markup("[red]暂不支持[/]");
                            break;
                    }
                    break;
                case ProjectType.Digital:
                    switch (refInfo.Type)
                    {
                        case ProjectType.Memory:
                        case ProjectType.Ip:
                            newPjInfo = AddRefToProjectInfo(info, refInfo, settings.Reference);
                            break;
                        default:
                            AnsiConsole.Markup("[red]暂不支持[/]");
                            break;
                    }
                    break;
                default:
                    AnsiConsole.Markup("[red]暂不支持[/]");
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
                    AnsiConsole.Markup("[red]已经添加过同名的项目[/]");
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
                    Path = reference
                });

                return projectInfo;
            }
            else
            {
                projectInfo.Reference = new List<ProjectReference>
                {
                    new ProjectReference
                    {
                        Name = refProjectInfo.Project,
                        Path = reference
                    }
                };
                return projectInfo;
            }
        }
    }
}
