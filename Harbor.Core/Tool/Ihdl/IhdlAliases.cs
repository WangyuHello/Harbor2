using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Harbor.Core.Tool.Ihdl
{
    public static class IhdlAliases
    {
        [CakeMethodAlias]
        public static void Ihdl(this ICakeContext context, FilePath param, string destIRLib, FilePath verilog, DirectoryPath directory = null)
        {
            var configure = new IhdlRunnerSettings
            {
                Param = param,
                DestIRLib = destIRLib,
                Verilog = verilog
            };
            if (directory !=null)
            {
                configure.WorkingDirectory = directory;
            }

            Ihdl(context, configure);
        }

        [CakeMethodAlias]
        public static void Ihdl(this ICakeContext context, FilePath param, FilePath verilog, DirectoryPath directory = null)
        {
            var configure = new IhdlRunnerSettings
            {
                Param = param,
                Verilog = verilog
            };
            if (directory != null)
            {
                configure.WorkingDirectory = directory;
            }

            Ihdl(context, configure);
        }

        [CakeMethodAlias]
        public static void Ihdl(this ICakeContext context, IhdlRunnerSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var runner = new IhdlRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(settings, context);
        }
    }
}
