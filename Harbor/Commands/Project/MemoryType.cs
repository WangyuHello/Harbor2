using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Commands.Project
{
    public enum MemoryType
    {
        ROM,
        Register,
        SRAM
    }

    public enum MemoryPortType
    {
        Single,
        Two,
        Dual
    }
}
