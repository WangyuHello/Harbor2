from pyverilog.vparser.parser import parse
import pyverilog.vparser.ast as vast
top_ports = []
def gen_ports_in_ports(portlist):
    ports = list(portlist.ports)
    for p in ports:
        if(p.__class__ == vast.Ioport):
            if(p.first.width != None):
                w = p.first.width
                top_ports.append({"Name": p.first.name, "Width": {"msb": int(w.msb.value), "lsb": int(w.lsb.value)}})
            else:
                top_ports.append({"Name": p.first.name, "Width": {"msb": 0, "lsb": 0}})
def gen_ports_in_items(items):
    items = list(items)
    for i in items:
        if(isinstance(i, vast.Decl)):
            l = i.list
            for ll in l:
                if(isinstance(ll, vast.Input) or isinstance(ll, vast.Output)):
                    if(ll.width != None):
                        w = ll.width
                        top_ports.append({"Name": ll.name, "Width": {"msb": int(w.msb.value), "lsb": int(w.lsb.value)}})
                    else:
                        top_ports.append({"Name": ll.name, "Width": {"msb": 0, "lsb": 0}})
def get_ports(node):
    for i in node.children():
        i_name = i.__class__.__name__
        if i_name == "ModuleDef":
            if i.name == top:
                gen_ports_in_ports(i.portlist)
                gen_ports_in_items(i.items)
        get_ports(i)
ast, directives = parse([filename], preprocess_include=[], preprocess_define=[])
get_ports(ast)