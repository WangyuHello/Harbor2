from pyverilog.vparser.parser import parse
import pyverilog.vparser.ast as vast
from pyverilog.ast_code_generator.codegen import ASTCodeGenerator

def convert_rtl_to_ams(node):
    for i in node.children():
        if i.__class__.__name__ == 'ModuleDef':
            module_name = i.name
            if module_name == top:
                pass
            else:
                i.name = module_name + "_AMS"
                print(i.name)
        elif i.__class__.__name__ == 'Instance':
            # i.show()
            i.module = i.module + "_AMS"
        elif i.__class__.__name__ == 'InstanceList':
            i.module = i.module + "_AMS"

        convert_rtl_to_ams(i)


src_ast, src_directives = parse([filename], preprocess_include=[], preprocess_define=[])

convert_rtl_to_ams(src_ast)

codegen = ASTCodeGenerator()
rslt = codegen.visit(src_ast)
