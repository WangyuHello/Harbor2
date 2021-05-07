using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Harbor.Core.Tool.ICC
{
    public class ICCRunnerSettings : HarborToolSettings
    {
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
        //-psyn_mode(run the shell with psyn_shell command subset (optional))
        //-xp_mode(run the shell in xp_mode (optional))
        //-dp_mode(run the shell in dp_mode (optional))
        //-ag_mode(run the shell in ag_mode (optional))
        //-shared_license(run the shell in shared licensing (optional))
        //-gui(run shell with gui (optional))
        //-no_gui(run shell without gui (optional))

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
        public bool PSynMode { get; set; }
        public bool XPMode { get; set; }
        public bool DPMode { get; set; }
        public bool AGMode { get; set; }
        public bool SharedLicense { get; set; }
        public bool GUI { get; set; }
        public bool NOGUI { get; set; }

        internal override void Evaluate(ProcessArgumentBuilder args)
        {
            if (!string.IsNullOrWhiteSpace(SynopsysRootPath))
                args.Append($"-r {SynopsysRootPath}");
            if (!string.IsNullOrWhiteSpace(CommandFile.FullPath))
                args.Append($"-f {CommandFile.FullPath}");
            if (!string.IsNullOrWhiteSpace(Command))
                args.Append($"-x {Command}");
            if (NoInit)
                args.Append("-no_init");
            if (NoHomeInit)
                args.Append("-no_home_init");
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
            if (PSynMode)
                args.Append("-psyn_mode");
            if (XPMode)
                args.Append("-xp_mode");
            if (DPMode)
                args.Append("-dp_mode");
            if (AGMode)
                args.Append("-ag_mode");
            if (SharedLicense)
                args.Append("-shared_license");
            if (GUI)
                args.Append("-gui");
            if (NOGUI)
                args.Append("-no_gui");
        }
    }

    public class ICCRunner : HarborTool<ICCRunnerSettings>
    {
        public ICCRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
            
        }

        protected override IEnumerable<string> GetToolExecutableNames() => new[] { "icc_shell" };

        protected override string GetToolName() => "IC Compiler";
    }
}
