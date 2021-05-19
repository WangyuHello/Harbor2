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
        public string Header =>
            "#------------------------------------------------" + Environment.NewLine +
            "# 自动生成的脚本" + Environment.NewLine +
            "# Harbor " + VersionResolver.GetVersion2() + Environment.NewLine +
            "# " + DateTime.Now + Environment.NewLine +
            "#------------------------------------------------";
    }
}
