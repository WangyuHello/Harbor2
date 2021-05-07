using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Newtonsoft.Json.Linq;

namespace Harbor.Core
{
    public class HarborToolSettings : ToolSettings
    {
        public FilePath CommandLogFile { get; set; }
        public ICakeContext Context { get; set; }
        public JObject ProjectInfo { get; set; }
        internal virtual void Evaluate(ProcessArgumentBuilder args)
        {
            
        }

        internal virtual void GenerateTclScripts() { }
    }
}
