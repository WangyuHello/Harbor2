﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本: 16.0.0.0
//  
//     对此文件的更改可能导致不正确的行为，如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Harbor.Core.Tool.APR.Tcl
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using Harbor.Core.Tool.APR.Model;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class PinPadTcl : PinPadTclBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("#自动生成的脚本\r\n#");
            
            #line 8 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(System.DateTime.Now));
            
            #line default
            #line hidden
            this.Write("\r\n#=============左侧端口====================\r\n");
            
            #line 10 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
  
int space = (int)model.PinSpace;
var realLeftOrders = 1; 
decimal offset = 0m; 

            
            #line default
            #line hidden
            
            #line 15 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 foreach(var p in model.LeftPorts) { 
            
            #line default
            #line hidden
            
            #line 16 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 for(int i = p.Width.lsb; i <= p.Width.msb; i++) { 
            
            #line default
            #line hidden
            
            #line 17 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 offset += model.PinSpace;
var tempName = $"{p.Name}[{i}]";
if((p.Width.msb == p.Width.lsb) && (p.Width.msb == 0))
{
	tempName = p.Name;
}

            
            #line default
            #line hidden
            
            #line 24 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 if(model.PinPlaceMode == PinPlaceMode.Uniform ) { 
            
            #line default
            #line hidden
            this.Write("set_pin_physical_constraints -pin_name {");
            
            #line 25 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tempName));
            
            #line default
            #line hidden
            this.Write("} -layers {metal");
            
            #line 25 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(p.MetalLayer));
            
            #line default
            #line hidden
            this.Write("} -side ");
            
            #line 25 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture((int)p.Position+1));
            
            #line default
            #line hidden
            this.Write(" -order ");
            
            #line 25 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(realLeftOrders));
            
            #line default
            #line hidden
            this.Write(" -pin_spacing ");
            
            #line 25 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(space));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 26 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } else if(model.PinPlaceMode == PinPlaceMode.ByOffset) { 
            
            #line default
            #line hidden
            this.Write("set_pin_physical_constraints -pin_name {");
            
            #line 27 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tempName));
            
            #line default
            #line hidden
            this.Write("} -layers {metal");
            
            #line 27 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(p.MetalLayer));
            
            #line default
            #line hidden
            this.Write("} -side ");
            
            #line 27 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture((int)p.Position+1));
            
            #line default
            #line hidden
            this.Write(" -order ");
            
            #line 27 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(realLeftOrders));
            
            #line default
            #line hidden
            this.Write(" -pin_spacing 0 -offset ");
            
            #line 27 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(offset));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 28 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } 
            
            #line default
            #line hidden
            
            #line 29 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 realLeftOrders += 1; 
            
            #line default
            #line hidden
            
            #line 30 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } 
            
            #line default
            #line hidden
            
            #line 31 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } 
            
            #line default
            #line hidden
            this.Write("#=============上侧端口====================\r\n");
            
            #line 33 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 var realTopOrders = 1; 
offset = 0m;

            
            #line default
            #line hidden
            
            #line 36 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 foreach(var p in model.TopPorts) { 
            
            #line default
            #line hidden
            
            #line 37 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 for(int i = p.Width.lsb; i <= p.Width.msb; i++) { 
            
            #line default
            #line hidden
            
            #line 38 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"

offset += model.PinSpace;
var tempName = $"{p.Name}[{i}]";
if((p.Width.msb == p.Width.lsb) && (p.Width.msb == 0))
{
tempName = p.Name;
}

            
            #line default
            #line hidden
            
            #line 46 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 if(model.PinPlaceMode == PinPlaceMode.Uniform ) { 
            
            #line default
            #line hidden
            this.Write("set_pin_physical_constraints -pin_name {");
            
            #line 47 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tempName));
            
            #line default
            #line hidden
            this.Write("} -layers {metal");
            
            #line 47 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(p.MetalLayer));
            
            #line default
            #line hidden
            this.Write("} -side ");
            
            #line 47 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture((int)p.Position+1));
            
            #line default
            #line hidden
            this.Write(" -order ");
            
            #line 47 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(realTopOrders));
            
            #line default
            #line hidden
            this.Write(" -pin_spacing ");
            
            #line 47 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(space));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 48 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } else if(model.PinPlaceMode == PinPlaceMode.ByOffset) { 
            
            #line default
            #line hidden
            this.Write("set_pin_physical_constraints -pin_name {");
            
            #line 49 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tempName));
            
            #line default
            #line hidden
            this.Write("} -layers {metal");
            
            #line 49 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(p.MetalLayer));
            
            #line default
            #line hidden
            this.Write("} -side ");
            
            #line 49 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture((int)p.Position+1));
            
            #line default
            #line hidden
            this.Write(" -order ");
            
            #line 49 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(realTopOrders));
            
            #line default
            #line hidden
            this.Write(" -pin_spacing 0 -offset ");
            
            #line 49 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(offset));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 50 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } 
            
            #line default
            #line hidden
            
            #line 51 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 realTopOrders += 1; 
            
            #line default
            #line hidden
            
            #line 52 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } 
            
            #line default
            #line hidden
            
            #line 53 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } 
            
            #line default
            #line hidden
            this.Write("#=============右侧端口====================\r\n");
            
            #line 55 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 var realRightOrders = 1; 
offset = 0m;

            
            #line default
            #line hidden
            
            #line 58 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 foreach(var p in model.RightPorts) { 
            
            #line default
            #line hidden
            
            #line 59 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 for(int i = p.Width.lsb; i <= p.Width.msb; i++) { 
            
            #line default
            #line hidden
            
            #line 60 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"

offset += model.PinSpace;
var tempName = $"{p.Name}[{i}]";
if((p.Width.msb == p.Width.lsb) && (p.Width.msb == 0))
{
tempName = p.Name;
}

            
            #line default
            #line hidden
            
            #line 68 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 if(model.PinPlaceMode == PinPlaceMode.Uniform ) { 
            
            #line default
            #line hidden
            this.Write("set_pin_physical_constraints -pin_name {");
            
            #line 69 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tempName));
            
            #line default
            #line hidden
            this.Write("} -layers {metal");
            
            #line 69 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(p.MetalLayer));
            
            #line default
            #line hidden
            this.Write("} -side ");
            
            #line 69 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture((int)p.Position+1));
            
            #line default
            #line hidden
            this.Write(" -order ");
            
            #line 69 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(realRightOrders));
            
            #line default
            #line hidden
            this.Write(" -pin_spacing ");
            
            #line 69 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(space));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 70 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } else if(model.PinPlaceMode == PinPlaceMode.ByOffset) { 
            
            #line default
            #line hidden
            this.Write("set_pin_physical_constraints -pin_name {");
            
            #line 71 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tempName));
            
            #line default
            #line hidden
            this.Write("} -layers {metal");
            
            #line 71 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(p.MetalLayer));
            
            #line default
            #line hidden
            this.Write("} -side ");
            
            #line 71 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture((int)p.Position+1));
            
            #line default
            #line hidden
            this.Write(" -order ");
            
            #line 71 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(realRightOrders));
            
            #line default
            #line hidden
            this.Write(" -pin_spacing 0 -offset ");
            
            #line 71 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(offset));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 72 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } 
            
            #line default
            #line hidden
            
            #line 73 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 realRightOrders += 1; 
            
            #line default
            #line hidden
            
            #line 74 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } 
            
            #line default
            #line hidden
            
            #line 75 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } 
            
            #line default
            #line hidden
            this.Write("#=============下侧端口====================\r\n");
            
            #line 77 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 var realBottomOrders = 1; 
offset = 0m;

            
            #line default
            #line hidden
            
            #line 80 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 foreach(var p in model.BottomPorts) { 
            
            #line default
            #line hidden
            
            #line 81 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 for(int i = p.Width.lsb; i <= p.Width.msb; i++) { 
            
            #line default
            #line hidden
            
            #line 82 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"

offset += model.PinSpace;
var tempName = $"{p.Name}[{i}]";
if((p.Width.msb == p.Width.lsb) && (p.Width.msb == 0))
{
tempName = p.Name;
}

            
            #line default
            #line hidden
            
            #line 90 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 if(model.PinPlaceMode == PinPlaceMode.Uniform ) { 
            
            #line default
            #line hidden
            this.Write("set_pin_physical_constraints -pin_name {");
            
            #line 91 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tempName));
            
            #line default
            #line hidden
            this.Write("} -layers {metal");
            
            #line 91 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(p.MetalLayer));
            
            #line default
            #line hidden
            this.Write("} -side ");
            
            #line 91 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture((int)p.Position+1));
            
            #line default
            #line hidden
            this.Write(" -order ");
            
            #line 91 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(realBottomOrders));
            
            #line default
            #line hidden
            this.Write(" -pin_spacing ");
            
            #line 91 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(space));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 92 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } else if(model.PinPlaceMode == PinPlaceMode.ByOffset) { 
            
            #line default
            #line hidden
            this.Write("set_pin_physical_constraints -pin_name {");
            
            #line 93 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tempName));
            
            #line default
            #line hidden
            this.Write("} -layers {metal");
            
            #line 93 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(p.MetalLayer));
            
            #line default
            #line hidden
            this.Write("} -side ");
            
            #line 93 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture((int)p.Position+1));
            
            #line default
            #line hidden
            this.Write(" -order ");
            
            #line 93 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(realBottomOrders));
            
            #line default
            #line hidden
            this.Write(" -pin_spacing 0 -offset ");
            
            #line 93 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(offset));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 94 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } 
            
            #line default
            #line hidden
            
            #line 95 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 realBottomOrders += 1; 
            
            #line default
            #line hidden
            
            #line 96 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } 
            
            #line default
            #line hidden
            
            #line 97 "E:\Documents\Repo\Harbor2\Harbor.Core\Tool\APR\Tcl\PinPadTcl.tt"
 } 
            
            #line default
            #line hidden
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
    public class PinPadTclBase
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