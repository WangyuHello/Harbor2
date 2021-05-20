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
            return comment + "------------------------------------------------" + Environment.NewLine +
                   comment + " 自动生成的脚本" + Environment.NewLine +
                   comment + " Harbor " + VersionResolver.GetVersion2() + Environment.NewLine +
                   comment + " " + DateTime.Now + Environment.NewLine +
                   comment + "------------------------------------------------";
        }

    }
}
