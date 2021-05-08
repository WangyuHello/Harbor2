using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cake.Core;
using Cake.Core.Annotations;
using Harbor.Core.Project;

namespace Harbor.Core.Tool.Project
{
    public static class ProjectAliases
    {
        [CakeMethodAlias]
        public static ProjectInfo ReadProject(this ICakeContext context)
        {
            return ProjectInfo.ReadFromContext(context);
        }

        [CakeMethodAlias]
        public static void CreateAnalogProject(this ICakeContext context, string name, Library library)
        {

        }
    }
}
