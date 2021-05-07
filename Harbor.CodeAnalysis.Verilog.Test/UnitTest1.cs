using System;
using System.Diagnostics;
using Xunit;

namespace Harbor.CodeAnalysis.Verilog.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var file = @"C:\Users\wangyu\Desktop\Temp\test.v";
            VerilogParser parser = new VerilogParser(file);
            parser.Parse();
            parser.Show();
            Debug.WriteLine(parser);
        }
    }
}
