using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cake.Core;
using Cake.Core.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Harbor.Core.Tool.Project
{
    public static class ProjectAliases
    {
        [CakeMethodAlias]
        [CakeNamespaceImport("Newtonsoft.Json.Linq")]
        public static JObject ReadProject(this ICakeContext context)
        {
            var curDir = context.Environment.WorkingDirectory;
            var obj = JObject.Load(new JsonTextReader(new StreamReader(curDir.CombineWithFilePath("project.json").FullPath)));
            
            return obj;
        }
    }
}
