using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harbor.Commands.Project;
using Harbor.Commands.Template;

namespace Harbor.Commands.Util
{
    public class CdsUtil
    {
        public static async Task CreateCdsLibAsync(string dir, Library.LibraryPdk pdk, List<Library.LibraryStdCell> stdCell, List<Library.LibraryIo> io)
        {
            var cdsLib = new CdsLib
            {
                Pdk = pdk,
                StdCell = stdCell,
                Io = io
            };
            var text = cdsLib.TransformText();
            await File.WriteAllTextAsync(Path.Combine(dir, "cds.lib"), text.Replace("\r",""), encoding: Encoding.UTF8);

            var cdsInit = new CdsInit
            {
                Pdk = pdk
            };
            text = cdsInit.TransformText();
            await File.WriteAllTextAsync(Path.Combine(dir, ".cdsinit"), text.Replace("\r", ""), encoding: Encoding.UTF8);

            var cdsEnv = new CdsEnv();
            text = cdsEnv.TransformText();
            await File.WriteAllTextAsync(Path.Combine(dir, ".cdsenv"), text.Replace("\r", ""), encoding: Encoding.UTF8);

            var simRc = new SimRc();
            text = simRc.TransformText();
            await File.WriteAllTextAsync(Path.Combine(dir, ".simrc"), text.Replace("\r", ""), encoding: Encoding.UTF8);
        }
    }
}
