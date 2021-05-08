using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harbor.Core.Tool.GetPorts;
using Xunit;

namespace Harbor.Test
{
    public class PythonTest
    {
        [Fact]
        public void Test1()
        {
            GetPorts.Run(@"C:\Users\wangyu\OneDrive\实验室\SRAM\Source\Chip.v");
        }
    }
}
