using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core;

namespace Harbor.Core.Tool.Syn
{
    public static class SynExitHandler
    {
        public static void Handle(int exitCode)
        {
            switch (exitCode)
            {
                case 1:
                    throw new CakeException("分析Verilog文件发生错误");
                case 2:
                    throw new CakeException("Elaborate发生错误");
                case 3:
                    throw new CakeException("Link 发生错误，可能含有未找到的引用");
                case 4:
                    throw new CakeException("Uniquify 发生错误");
                case 5:
                    throw new CakeException("综合发生错误");
            }
        }
    }
}
