﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本: 16.0.0.0
//  
//     对此文件的更改可能导致不正确的行为，如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Harbor.Core.Tool.Syn.Tcl
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using Harbor.Core.Util;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class BuildTcl : BuildTclBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            
            #line 7 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(HarborTextModel.Header()));
            
            #line default
            #line hidden
            this.Write("\r\n\r\n#=====================设置变量======================\r\nset script_root_path \"");
            
            #line 10 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.ScriptRootPath));
            
            #line default
            #line hidden
            this.Write("\"\r\nset wrk_path         \"");
            
            #line 11 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.WorkPath));
            
            #line default
            #line hidden
            this.Write("\"\r\nset rpt_path         \"");
            
            #line 12 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.RptPath));
            
            #line default
            #line hidden
            this.Write("\"\r\nset net_path         \"");
            
            #line 13 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.NetPath));
            
            #line default
            #line hidden
            this.Write("\"\r\nset svf_path         \"");
            
            #line 14 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.SvfPath));
            
            #line default
            #line hidden
            this.Write("\"\r\nset obj_path         \"");
            
            #line 15 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.ObjPath));
            
            #line default
            #line hidden
            this.Write("\"\r\nset lib_path         \"");
            
            #line 16 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.LibPath));
            
            #line default
            #line hidden
            this.Write("\"\r\n\r\nset search_path \"$script_root_path \\\r\n                 $lib_path         \\\r\n" +
                    "                \"\r\n\r\nset cache_write       \"/tmp\"\r\nset cache_read        \"/tmp\"\r" +
                    "\n\r\nhistory keep 500\r\n\r\nset_host_options -max_core ");
            
            #line 27 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.Cores));
            
            #line default
            #line hidden
            this.Write("\r\n\r\n#=====================设置工艺库======================\r\n\r\nset lib_name       \"");
            
            #line 31 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.LibName));
            
            #line default
            #line hidden
            this.Write("\"\r\nset target_library \"");
            
            #line 32 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.LibFullName));
            
            #line default
            #line hidden
            this.Write(" \\\r\n");
            
            #line 33 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 foreach(var io_lib in model.IOTimingDbPaths) { 
            
            #line default
            #line hidden
            this.Write("                    ");
            
            #line 34 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(io_lib));
            
            #line default
            #line hidden
            this.Write(" \\\r\n");
            
            #line 35 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } 
            
            #line default
            #line hidden
            
            #line 36 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 foreach(var addi_lib in model.AdditionalTimingDbPaths) { 
            
            #line default
            #line hidden
            this.Write("                    ");
            
            #line 37 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(addi_lib));
            
            #line default
            #line hidden
            this.Write(" \\\r\n");
            
            #line 38 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } 
            
            #line default
            #line hidden
            this.Write("                   \"\r\nset link_library   \"");
            
            #line 40 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.LibFullName));
            
            #line default
            #line hidden
            this.Write(" \\\r\n");
            
            #line 41 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 foreach(var io_lib in model.IOTimingDbPaths) { 
            
            #line default
            #line hidden
            this.Write("                    ");
            
            #line 42 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(io_lib));
            
            #line default
            #line hidden
            this.Write(" \\\r\n");
            
            #line 43 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } 
            
            #line default
            #line hidden
            
            #line 44 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 foreach(var addi_lib in model.AdditionalTimingDbPaths) { 
            
            #line default
            #line hidden
            this.Write("                    ");
            
            #line 45 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(addi_lib));
            
            #line default
            #line hidden
            this.Write(" \\\r\n");
            
            #line 46 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } 
            
            #line default
            #line hidden
            this.Write("                   \"\r\n");
            
            #line 48 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 if(!string.IsNullOrEmpty(model.StdCell.symbol_full_name)) { 
            
            #line default
            #line hidden
            this.Write("set symbol_library \"");
            
            #line 49 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.StdCell.symbol_full_name));
            
            #line default
            #line hidden
            this.Write("\"\r\n");
            
            #line 50 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } 
            
            #line default
            #line hidden
            this.Write(@"
#=====================设置项目====================

#设置项目临时目录 
define_design_lib DEFAULT -path ""$obj_path""

#设置项目目录
define_design_lib work -path ""$wrk_path""

#设置命名规则:禁止小写
define_name_rules BORG -allowed {A-Za-z0-9_} -first_restricted ""_"" -last_restricted ""_"" -max_length 30
define_name_rules ""IS_rule"" -max_length ""255"" -allowed ""A-Z0-9_$""  -replacement_char ""_"" -type cell
define_name_rules ""IS_rule"" -max_length ""255"" -allowed ""A-Z0-9_$""  -replacement_char ""_"" -type net
define_name_rules ""IS_rule"" -max_length ""255"" -allowed ""A-Z0-9_$[]""  -replacement_char ""_"" -type port

");
            
            #line 66 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 if(!model.AllowTriState) {
            
            #line default
            #line hidden
            this.Write("#消除三态门\r\nset verilogout_no_tri true\r\n");
            
            #line 69 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n#=================导入Verilog源代码===================\r\n\r\nset top_name \"");
            
            #line 73 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.TopName));
            
            #line default
            #line hidden
            this.Write("\"\r\n\r\nset_svf      \"${svf_path}/${top_name}.svf\"\r\n\r\n");
            
            #line 77 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 foreach(var v in model?.SourceFullPaths) { 
            
            #line default
            #line hidden
            this.Write("set r [analyze -format verilog -lib work ");
            
            #line 78 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(v));
            
            #line default
            #line hidden
            this.Write("]\r\nif { $r == 0 } {\r\n    exit 1\r\n}\r\n");
            
            #line 82 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } 
            
            #line default
            #line hidden
            this.Write(@"
set r [elaborate $top_name -arch ""verilog"" -lib work]
if { $r == 0 } {
    exit 2
}

current_design $top_name

set r [link]
if { $r == 0 } {
    exit 3
}

set r [uniquify]
if { $r == 0 } {
    exit 4
}


change_names -rules ""IS_rule"" -hierarchy

#======================设置约束======================

#======时钟设置========

set clk_name """);
            
            #line 108 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.ClkName));
            
            #line default
            #line hidden
            this.Write("\"\r\n");
            
            #line 109 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 if(!string.IsNullOrEmpty(model.RstName)) {
            
            #line default
            #line hidden
            this.Write("set rst_name \"");
            
            #line 110 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.RstName));
            
            #line default
            #line hidden
            this.Write("\"\r\n");
            
            #line 111 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\r\ncreate_clock -name $clk_name -period ");
            
            #line 113 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.ClkPeriod));
            
            #line default
            #line hidden
            this.Write(" -waveform {0 ");
            
            #line 113 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.ClkPeriod / 2));
            
            #line default
            #line hidden
            this.Write(" } [get_ports $clk_name]\r\n\r\nset_dont_touch_network [get_clocks $clk_name]\r\n\r\nset_" +
                    "clock_latency ");
            
            #line 117 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.ClkLatency));
            
            #line default
            #line hidden
            this.Write(" [get_clocks $clk_name]\r\nset_clock_uncertainty  ");
            
            #line 118 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.ClkUncertainty));
            
            #line default
            #line hidden
            this.Write(" [get_clocks $clk_name]\r\n\r\n");
            
            #line 120 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 
var removePorts = "$clk_name";
if(!string.IsNullOrEmpty(model.RstName)) {
    removePorts += " $rst_name";
}

            
            #line default
            #line hidden
            this.Write("\r\nset_input_delay -max ");
            
            #line 127 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.MaxInputDelay));
            
            #line default
            #line hidden
            this.Write(" -clock $clk_name [remove_from_collection [all_inputs] [get_ports \"");
            
            #line 127 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(removePorts));
            
            #line default
            #line hidden
            this.Write("\"]]\r\nset_input_delay -min ");
            
            #line 128 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.MinInputDelay));
            
            #line default
            #line hidden
            this.Write(" -clock $clk_name [remove_from_collection [all_inputs] [get_ports \"");
            
            #line 128 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(removePorts));
            
            #line default
            #line hidden
            this.Write("\"]]\r\nset_output_delay -max ");
            
            #line 129 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.MaxOutputDelay));
            
            #line default
            #line hidden
            this.Write(" -clock $clk_name [all_outputs]\r\nset_output_delay -min ");
            
            #line 130 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.MinOutputDelay));
            
            #line default
            #line hidden
            this.Write(" -clock $clk_name [all_outputs]\r\n\r\nset_fix_hold [get_clocks $clk_name]\r\n\r\n");
            
            #line 134 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 if(!string.IsNullOrEmpty(model.RstName)) {
            
            #line default
            #line hidden
            this.Write("set_ideal_network [get_ports \"$rst_name\"]\r\nset_false_path -from [get_ports \"$rst_" +
                    "name\"]\r\nset_dont_touch_network [get_ports \"$rst_name\"]\r\n");
            
            #line 138 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n#======================驱动和负载===================\r\n\r\n# 工艺库相关\r\nset inv_name       " +
                    "\"");
            
            #line 143 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.InvName));
            
            #line default
            #line hidden
            this.Write("\"\r\nset inv_port_name  \"");
            
            #line 144 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.InvPortName));
            
            #line default
            #line hidden
            this.Write("\"\r\n\r\nset unit_load [load_of $lib_name/$inv_name/$inv_port_name]\r\n\r\n# 无限驱动\r\nset_dr" +
                    "ive 0 $clk_name\r\n");
            
            #line 150 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 if(!string.IsNullOrEmpty(model.RstName)) {
            
            #line default
            #line hidden
            this.Write("set_drive 0 $rst_name\r\n");
            
            #line 152 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n# 单位反相器的驱动\r\n");
            
            #line 155 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 if(!string.IsNullOrEmpty(model.RstName)) {
            
            #line default
            #line hidden
            this.Write("set_driving_cell -lib_cell $inv_name  [remove_from_collection [all_inputs] [get_p" +
                    "orts \"$clk_name $rst_name\"]]\r\n");
            
            #line 157 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } else {
            
            #line default
            #line hidden
            this.Write("set_driving_cell -lib_cell $inv_name  [remove_from_collection [all_inputs] [get_p" +
                    "orts \"$clk_name\" ]]\r\n");
            
            #line 159 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n# set_max_capacitance [expr $unit_load*");
            
            #line 161 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.CapFactor));
            
            #line default
            #line hidden
            this.Write("] [all_designs]\r\nset_load [expr $unit_load*");
            
            #line 162 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.LoadFactor));
            
            #line default
            #line hidden
            this.Write("] [all_outputs]\r\n");
            
            #line 163 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 foreach(var p in model.PortSettings) { 
            
            #line default
            #line hidden
            
            #line 164 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 if(string.IsNullOrEmpty(p.LoadOf)) { 
            
            #line default
            #line hidden
            this.Write("set_load [expr $unit_load*");
            
            #line 165 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(p.LoadFactor));
            
            #line default
            #line hidden
            this.Write("] {\"");
            
            #line 165 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(p.Name.ToUpper()));
            
            #line default
            #line hidden
            this.Write("\"}\r\n");
            
            #line 166 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } else { 
            
            #line default
            #line hidden
            this.Write("set_load [load_of \"");
            
            #line 167 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(p.LoadOf));
            
            #line default
            #line hidden
            this.Write("\"] {\"");
            
            #line 167 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(p.Name.ToUpper()));
            
            #line default
            #line hidden
            this.Write("\"}\r\n");
            
            #line 168 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } } 
            
            #line default
            #line hidden
            this.Write("\r\nset_max_fanout ");
            
            #line 170 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.MaxFanout));
            
            #line default
            #line hidden
            this.Write(" [current_design]\r\nset_max_transition ");
            
            #line 171 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.MaxTransition));
            
            #line default
            #line hidden
            this.Write(@" [current_design]

report_constraint -max_capacitance -significant_digits 13
report_constraint -max_transition  -significant_digits 13
report_constraint -max_fanout      -significant_digits 13

#=====================其他约束=====================

set_max_area ");
            
            #line 179 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.MaxArea));
            
            #line default
            #line hidden
            this.Write("\r\nset_critical_range ");
            
            #line 180 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.CriticalRange));
            
            #line default
            #line hidden
            this.Write(" [get_designs *]\r\n\r\n#=====================综合=====================\r\n\r\n");
            
            #line 184 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 if(model.UseCompileUltra) {
            
            #line default
            #line hidden
            this.Write("set r [compile_ultra]\r\nif { $r == 0 } {\r\n    exit 5\r\n}\r\n");
            
            #line 189 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } else {
            
            #line default
            #line hidden
            this.Write("set r [compile]\r\nif { $r == 0 } {\r\n    exit 5\r\n}\r\n");
            
            #line 194 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
 } 
            
            #line default
            #line hidden
            this.Write(@"
#====================输出======================

check_design > $rpt_path/$top_name\_checkdesign.rpt

#输出网表
write -hierarchy -output                 $net_path/$top_name\.db
write -format ddc -hierarchy -output     $net_path/$top_name\.ddc
write -hierarchy -format verilog -output $net_path/$top_name\.v
write_sdc                                $net_path/$top_name\.sdc
write_sdf                                $net_path/$top_name\.sdf


#输出报告

report_constraint -all_violators -verbose -significant_digits 13  > $rpt_path/$top_name\_vio.rpt
report_timing -max ");
            
            #line 211 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\Syn\Tcl\BuildTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(model.TimingRptNum));
            
            #line default
            #line hidden
            this.Write(" > $rpt_path/$top_name\\_timing.rpt\r\nreport_area -hierarchy                       " +
                    "> $rpt_path/$top_name\\_area.rpt\r\nreport_power                                 > " +
                    "$rpt_path/$top_name\\_power.rpt\r\n\r\n#简报\r\nreport_constraint -all_violators\r\n\r\nquit");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class BuildTclBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
