from hdlConvertorAst.language import Language
from hdlConvertor import HdlConvertor
from hdlConvertorAst.hdlAst import HdlModuleDef
from hdlConvertorAst.to.verilog.verilog2005 import ToVerilog2005

c = HdlConvertor()
# note that there is also Language.VERILOG_2005, Language.VERILOG and others
src_ast = c.parse([filename], Language.SYSTEM_VERILOG_2017, include_dirs, hierarchyOnly=False, debug=True)

convert_rtl_to_ams(src_ast)