using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harbor.Common.Cli;

namespace Harbor.Core
{
    public class HarborTextModel
    {
        public static string Header(string comment = "#")
        {
            return comment + "------------------------------------------------" + System.Environment.NewLine +
                   comment + " 自动生成的脚本" + System.Environment.NewLine +
                   comment + " Harbor " + VersionResolver.GetVersion2() + System.Environment.NewLine +
                   comment + " " + DateTime.Now + System.Environment.NewLine +
                   comment + "------------------------------------------------";
        }

    }
}
