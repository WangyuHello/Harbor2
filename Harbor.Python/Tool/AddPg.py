from pyverilog.vparser.parser import parse
import pyverilog.vparser.ast as vast
from pyverilog.ast_code_generator.codegen import ASTCodeGenerator

zero_counter = 0

def add_power_for_port(portlist):
    dvdd = vast.Port(name='DVDD', width=None, dimensions=1, type=None, lineno=-1)
    dvss = vast.Port(name='DVSS', width=None, dimensions=1, type=None, lineno=-1)
    ports = list(portlist.ports)
    ports.append(dvdd)
    ports.append(dvss)
    portlist.ports= tuple(ports)

def add_power_for_decl(i):
    items = list(i.items)
    dvdd = vast.Input('DVDD')
    dvss = vast.Input('DVSS')
    inputs = [dvdd, dvss]
    decl = vast.Decl(inputs, lineno=-1)
    items.insert(0, decl)
    i.items = tuple(items)

def add_power_for_lib_instance(instance):
    ports = list(instance.portlist)
    _DVDD = vast.Identifier(name='DVDD', lineno=-1)
    _DVSS = vast.Identifier(name='DVSS', lineno=-1)
    _VDD = vast.PortArg(lib_ins_power_pin, _DVDD, lineno=-1)
    _VNW = vast.PortArg('VNW', _DVDD, lineno=-1)
    _VSS = vast.PortArg(lib_ins_ground_pin, _DVSS, lineno=-1)
    _VPW = vast.PortArg('VPW', _DVSS, lineno=-1)

    if primary_stdcell_name.startswith("scc"): # SMIC Std Cell 含有VNW/VPW接口
        ports.extend([_VDD, _VNW, _VSS, _VPW])
    else:
        ports.extend([_VDD, _VSS])
           
    instance.portlist = tuple(ports)

def add_power_for_user_instance(instance):
    ports = list(instance.portlist)
    _DVDD = vast.Identifier(name='DVDD', lineno=-1)
    _DVSS = vast.Identifier(name='DVSS', lineno=-1)
    _VDD = vast.PortArg('DVDD', _DVDD, lineno=-1)
    _VSS = vast.PortArg('DVSS', _DVSS, lineno=-1)
    ports.extend([_VDD, _VSS])
    instance.portlist = tuple(ports)

def add_power_for_macro_instance(instance):
    module_name = instance.module
    power_pins = macro_power_pins[module_name]["power_pins"]
    ground_pins = macro_power_pins[module_name]["ground_pins"]

    ports = list(instance.portlist)
    _DVDD = vast.Identifier(name='DVDD', lineno=-1)
    _DVSS = vast.Identifier(name='DVSS', lineno=-1)

    for p in power_pins:
        _p = vast.PortArg(p, _DVDD, lineno=-1)
        ports.append(_p)

    for g in ground_pins:
        _g = vast.PortArg(g, _DVSS, lineno=-1)
        ports.append(_g)

    instance.portlist = tuple(ports)


def convert_1b0_to_wire(i):
    global zero_counter
    c1 = i.argname
    c1_name = c1.__class__.__name__
    # print(c1_name)
    if c1_name == "IntConst":
        if c1.value == "1'b0":
            zero_counter += 1
            i.argname = vast.Identifier("HARBOR_ZERO_" + str(zero_counter))
    elif c1_name == "Concat":
        l1 = list(c1.list)
        for ind in range(len(l1)):
            c2 = l1[ind]
            c2_name = c2.__class__.__name__
            if c2_name == "IntConst" and c2.value == "1'b0":
                zero_counter += 1
                l1[ind] = vast.Identifier("HARBOR_ZERO_" + str(zero_counter))
        c1.list = tuple(l1)

def add_power(node):
    for i in node.children():
        i_name = i.__class__.__name__
        if i_name == "ModuleDef":
            # 在 ModuleDef 中添加DVDD,DVSS 
            add_power_for_port(i.portlist)
            add_power_for_decl(i)
        elif i_name == "Instance":
            # 在 InstanceList 中添加DVDD, 
            ins_module_name = i.module
            if(ins_module_name in lib_ins_list):
                add_power_for_lib_instance(i)
            elif(ins_module_name in macro_power_pins):
                add_power_for_macro_instance(i)
            else:
                add_power_for_user_instance(i)
        elif i_name == "PortArg":
            convert_1b0_to_wire(i)

        add_power(i)


ast, directives = parse([filename], preprocess_include=[], preprocess_define=[])

add_power(ast)
        
codegen = ASTCodeGenerator()
rslt = codegen.visit(ast)
