using System;
using System.Collections.Generic;
using System.Text;
using Harbor.CodeAnalysis.Verilog.Template;

namespace Harbor.CodeAnalysis.Verilog
{
    public class VerilogVisitor : SysVerilogHDLBaseVisitor<string>
    {
        public override string VisitModule_declaration(SysVerilogHDLParser.Module_declarationContext context)
        {
            var t = new ModuleDeclaration
            {
                PortList = VisitModule_interface(context.module_interface()),
                ModuleName = context.module_identifier().identifier().GetText()
            };
            return t.ToLfString();
        }

        public override string VisitModule_interface(SysVerilogHDLParser.Module_interfaceContext context)
        {
            return string.Join(",\n", VisitChildren(context));
        }

        public override string VisitPort_declaration(SysVerilogHDLParser.Port_declarationContext context)
        {
            var name = "";
            var direction = "";
            var hasDimension = false;
            var dimension = (0, 0);
            var reg = false;

            switch (context.GetChild(0))
            {
                case SysVerilogHDLParser.Input_declarationContext i:
                    direction = "input";
                    var portDescription = i.input_description().port_description();
                    switch (portDescription.GetChild(0))
                    {
                        case SysVerilogHDLParser.Dimension_plusContext d:
                            hasDimension = true;
                            dimension.Item1 = int.Parse(d.dimension()[0]
                                .range_expression()
                                .sb_range()
                                .base_expression()
                                .expression()
                                .single_expression()
                                .primary()
                                .number()
                                .integral_number().GetText());
                            dimension.Item2 = int.Parse(d.dimension()[0]
                                .range_expression()
                                .sb_range()
                                .expression()
                                .single_expression()
                                .primary()
                                .number()
                                .integral_number().GetText());
                            break;
                        case SysVerilogHDLParser.List_of_variable_descriptionsContext l:
                            break;
                    }

                    name = portDescription.list_of_variable_descriptions().variable_description().variable_identifier()
                        .identifier().GetText();
                    break;
                case SysVerilogHDLParser.Output_declarationContext o:
                    direction = "output";
                    var outputDescription = o.output_description();
                    switch (outputDescription.GetChild(0))
                    {
                        case SysVerilogHDLParser.Reg_declarationContext r:
                            reg = true;
                            name = r.list_of_variable_descriptions().variable_description().variable_identifier()
                                .identifier().GetText();
                            switch (r.GetChild(1))
                            {
                                case SysVerilogHDLParser.Dimension_plusContext d:
                                    hasDimension = true;
                                    dimension.Item1 = int.Parse(d.dimension()[0]
                                        .range_expression()
                                        .sb_range()
                                        .base_expression()
                                        .expression()
                                        .single_expression()
                                        .primary()
                                        .number()
                                        .integral_number().GetText());
                                    dimension.Item2 = int.Parse(d.dimension()[0]
                                        .range_expression()
                                        .sb_range()
                                        .expression()
                                        .single_expression()
                                        .primary()
                                        .number()
                                        .integral_number().GetText());
                                    break;
                            }
                            break;
                        case SysVerilogHDLParser.Port_descriptionContext p:
                            name = p.list_of_variable_descriptions().variable_description().variable_identifier()
                                .identifier().GetText();
                            switch (p.GetChild(0))
                            {
                                case SysVerilogHDLParser.Dimension_plusContext d:
                                    hasDimension = true;
                                    dimension.Item1 = int.Parse(d.dimension()[0]
                                        .range_expression()
                                        .sb_range()
                                        .base_expression()
                                        .expression()
                                        .single_expression()
                                        .primary()
                                        .number()
                                        .integral_number().GetText());
                                    dimension.Item2 = int.Parse(d.dimension()[0]
                                        .range_expression()
                                        .sb_range()
                                        .expression()
                                        .single_expression()
                                        .primary()
                                        .number()
                                        .integral_number().GetText());
                                    break;
                            }
                            break;
                    }
                    break;
            }

            if (!hasDimension)
            {
                return $"{direction} {(reg ? "reg " : "")}{name}";
            }

            return $"{direction} {(reg ? "reg " : "")}[{dimension.Item1}:{dimension.Item2}] {name}";
        }
    }
}
