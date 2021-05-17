from pyverilog.vparser.parser import parse
import pyverilog.vparser.ast as vast
from pyverilog.ast_code_generator.codegen import ASTCodeGenerator

VDD = 'DVDD'
VSS = 'DVSS'

def get_ports(node, port_lists):
    for i in node.children():
        i_name = i.__class__.__name__
        if i_name == "ModuleDef":
            module_name = i.name
            # print(module_name)
            portlist = i.portlist
            portlist = list(portlist.ports)
            real_port_list = []
            for p in portlist:
                if p.__class__.__name__ == 'Ioport': # 带有input或者output声明的
                    # print(p.first.name)
                    real_port_list.append(p.first.name)
                else: # 不带任何声明
                    # print(p.name)
                    real_port_list.append(p.name)

            port_lists[module_name] = real_port_list

        get_ports(i, port_lists)

def convert_ports_to_upper_in_function(node):
    for i in node.children():
        if i.__class__.__name__ == 'Input':
            i.name = i.name.upper()

        convert_ports_to_upper_in_function(i)


def convert_identifier_to_upper_in_module(src_ports, node):
    for i in node.children():
        i_name = i.__class__.__name__
        if i_name == "Identifier":
            if i.name in src_ports:
                i.name = i.name.upper()
            
        # elif i_name == "PortArg":
        #     i.portname = i.portname.upper()
        # elif i_name == "Wire":
        #     i.name = i.name.upper()
        # elif i_name == "Reg":
        #     i.name = i.name.upper()
        # elif i_name == "Integer":
        #     i.name = i.name.upper()
        # elif i_name == "Real":
        #     i.name = i.name.upper()
        # elif i_name == "Genvar":
        #     i.name = i.name.upper()

        convert_identifier_to_upper_in_module(src_ports, i)

def convert_ports_to_upper(src_ports, netlist_ports, node):
    for i in node.children():
        i_name = i.__class__.__name__
        if i_name == "ModuleDef":
            # 仅修改模块的Port, 并且根据netlist调整Port顺序
            module_name = i.name
            if module_name == top:
                # i.show()
                portlist = i.portlist
                portlist = list(portlist.ports)
                # print(portlist)
                new_order_port_list = []
                if module_name in netlist_ports: # 如果netlist含有这个模块,则调整顺序并大写
                    for target_ordered_port in netlist_ports[module_name]:
                        for origin_ordered_port in portlist:
                            origin_ordered_port_name = ''
                            if origin_ordered_port.__class__.__name__ == "Ioport":
                                origin_ordered_port_name = origin_ordered_port.first.name.upper()
                                origin_ordered_port.first.name = origin_ordered_port_name
                            else:
                                origin_ordered_port_name = origin_ordered_port.name.upper()
                                origin_ordered_port.name = origin_ordered_port_name
                            
                            if origin_ordered_port_name == target_ordered_port:
                                new_order_port_list.append(origin_ordered_port)
                else: # 如果netlist不不包含这个模块,则仅大写
                    for p in portlist:
                        if p.__class__.__name__ == "Ioport":
                            p.first.name = p.first.name.upper()
                        else:
                            p.name == p.name.upper()
                        new_order_port_list.append(p)

                
                dvdd = vast.Ioport(first = vast.Input(name = VDD), lineno=-1)
                dvss = vast.Ioport(first = vast.Input(name = VSS), lineno=-1)
                new_order_port_list.append(dvdd)
                new_order_port_list.append(dvss)
                i.portlist.ports = tuple(new_order_port_list)

                convert_identifier_to_upper_in_module(src_ports[top], i)

        convert_ports_to_upper(src_ports, netlist_ports, i)


src_ast, src_directives = parse([source], preprocess_include=[], preprocess_define=[])
netlist_ast, netlist_directives = parse([netlist], preprocess_include=[], preprocess_define=[])

src_ports = {}
get_ports(src_ast, src_ports)
netlist_ports = {}
get_ports(netlist_ast, netlist_ports)

convert_ports_to_upper(src_ports, netlist_ports, src_ast)

codegen = ASTCodeGenerator()
rslt = codegen.visit(src_ast)