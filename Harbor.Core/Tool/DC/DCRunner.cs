using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Harbor.Core.Tool.DC
{
    public class DCRunnerSettings : HarborToolSettings
    {
        //-shell % (Shell Name psyn_shell/dc_shell/dc_sms_shell/de_shell/lc_shell/ptxr)
        //-r % (synopsys root path)
        //-f % (execute_command file (optional))
        //-x % (command to execute(optional))
        //-no_init(don't load initialization files (optional))
        //-no_home_init (don't load home .synopsys file (optional))
        //-no_local_init (don't load local .synopsys file (optional))
        //-checkout % (check out these features (optional))
        //-timeout # (exit if license server fails to respond
        //            after these number of minutes(optional))
        //-wait # (exit if feature_list is not all available
        //         after these number of minutes(optional))
        //-version(show product version and exit immediately (optional))
        //-64bit(use 64-bit executable (optional))
        //-no_log(don't log commands (optional))
        //-output_log_file % (log console output to a file (optional))
        //-help(display this information)
        //-galaxy(Galileo license backward compatibility mode)
        //-gui(run shell with gui (optional))
        //-no_gui(run shell without gui (optional))
        //-topographical_mode(run shell in Topographical mode. By default, de_shell commands run in topographical mode)

        public DCShell Shell { get; set; }
        public string SynopsysRootPath { get; set; }
        public FilePath CommandFile { get; set; }
        public string Command { get; set; }
        public bool NoInit { get; set; }
        public bool NoHomeInit { get; set; }
        public bool NoLocalInit { get; set; }
        public string Checkout { get; set; }
        public int Timeout { get; set; }
        public int Wait { get; set; }
        public bool Version { get; set; }
        public bool Use64Bit { get; set; }
        public bool NoLog { get; set; }
        public string OutputLogFile { get; set; }
        public bool Help { get; set; }
        public bool Galaxy { get; set; }
        public bool GUI { get; set; }
        public bool NOGUI { get; set; }
        public bool TopographicalMode { get; set; }

        public enum DCShell
        {
            None,
            psyn_shell,
            dc_shell,
            dc_sms_shell,
            de_shell,
            lc_shell,
            ptxr
        }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            if (Shell != DCShell.None)
                args.Append($"-shell {Shell}");
            if (!string.IsNullOrWhiteSpace(SynopsysRootPath))
                args.Append($"-r {SynopsysRootPath}");
            if (!string.IsNullOrWhiteSpace(CommandFile.FullPath))
                args.Append($"-f {CommandFile.FullPath}");
            if (!string.IsNullOrWhiteSpace(Command))
                args.Append($"-x {Command}");
            if (NoInit)
                args.Append("-no_init");
            if (NoHomeInit)
                args.Append("-no_home_init"); if (NoHomeInit)
                if (NoLocalInit)
                    args.Append("-no_local_init");
            if (!string.IsNullOrWhiteSpace(Checkout))
                args.Append($"-checkout {Checkout}");
            if (Timeout > 0)
                args.Append($"-timeout {Timeout}");
            if (Wait > 0)
                args.Append($"-wait {Wait}");
            if (Version)
                args.Append("-version");
            if (Use64Bit)
                args.Append("-64bit");
            if (NoLog)
                args.Append("-no_log");
            if (!string.IsNullOrWhiteSpace(OutputLogFile))
                args.Append($"-output_log_file {OutputLogFile}");
            if (Help)
                args.Append("-help");
            if (Galaxy)
                args.Append("-galaxy");
            if (GUI)
                args.Append("-gui");
            if (NOGUI)
                args.Append("-no_gui");
            if (TopographicalMode)
                args.Append("-topographical_mode");
        }
    }

    public class DCRunner : HarborTool<DCRunnerSettings>
    {
        public DCRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {

        }

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "dc_shell" };

        protected override string GetToolName() => "Design Compiler";
    }
}
