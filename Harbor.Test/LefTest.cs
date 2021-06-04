using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harbor.Common.Util;
using Xunit;

namespace Harbor.Test
{
    public class LefTest
    {
        [Theory]
        [InlineData(@"C:\Users\wangyu\Desktop\Temp\CFPMAC2.lef")]
        [InlineData(@"C:\Users\wangyu\Desktop\Temp\CFPMAC2C.lef")]
        [InlineData(@"C:\Users\wangyu\Desktop\Temp\systolic_buffer_half.lef")]
        public void TestParse(string filename)
        {
            LefObject lef = LefObject.Parse(filename);
        }
    }
}
