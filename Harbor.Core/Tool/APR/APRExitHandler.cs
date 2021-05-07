using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core;

namespace Harbor.Core.Tool.APR
{
    public static class APRExitHandler
    {
        public static void HandleExit(int exitCode)
        {
            switch (exitCode)
            {
                case 1:
                    throw new CakeException("初始化项目失败");
                case 2:
                    throw new CakeException("初始化布局失败");
                case 3:
                    throw new CakeException("布局失败。可能原因：CoreUtilization太大。");
                case 4:
                case 5:
                    throw new CakeException("时钟树综合（CTS）失败");
                case 6:
                    throw new CakeException("时钟布线失败");
                case 7:
                case 8:
                    throw new CakeException("布线失败");
                case 9:
                    throw new CakeException("芯片完成阶段失败");
                default:
                    break;
            }
        }
    }
}
