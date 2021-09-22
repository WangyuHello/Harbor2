using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harbor.Core.Environment
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DefaultEnvironmentAttribute : Attribute
    {
    }
}
