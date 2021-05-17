using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harbor.Core.Tool.GetPorts;
using Harbor.Python.Tool;
using Xunit;

namespace Harbor.Test
{
    public class PythonTest
    {
        [Theory]
        [InlineData(@"C:\Users\wangyu\Desktop\Temp\SystemAgent.v")]
        public void TestGetPorts(string filename)
        {
            GetPorts.Run2(filename, "SystemAgent", @"C:\Users\wangyu\Desktop\Temp");
        }

        [Theory]
        [InlineData("SystemAgent", @"C:\Users\wangyu\Desktop\Temp\SystemAgent.v", @"C:\Users\wangyu\Desktop\Temp\SystemAgent_AMS.v")]
        public void TestConvertAMS(string top, string filename, string output)
        {
            ConvertAMS.Run2(top,filename, output, @"C:\Users\wangyu\Desktop\Temp");
        }
    }
}
