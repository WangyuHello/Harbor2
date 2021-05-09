using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Python.Runtime;

namespace Harbor.Python.Tool
{
    public static class ReadLef
    {
        public static JObject Run(string filename)
        {
            PythonHelper.SetEnvironment();
            JObject r;
            using (Py.GIL())
            {
                dynamic parser = Py.Import("LefParser.lef_parser");
                dynamic json = Py.Import("json");

                var lef_parser = parser.LefParser(filename);
                lef_parser.parse();
                var lef_json = json.dumps(lef_parser);
                r = JObject.Parse(lef_json.As<string>());
            }

            return r;
        }
    }
}
