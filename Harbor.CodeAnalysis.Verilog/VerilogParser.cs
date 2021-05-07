using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Harbor.CodeAnalysis.Verilog
{
    public class VerilogParser
    {
        public ParserRuleContext Tree { get; set; }
        public string Source { get; set; }
        private SysVerilogHDLParser _parser;

        public VerilogParser(string file)
        {
            Source = file;
        }

        public void Parse()
        {
            var content = File.ReadAllText(Source);
            var stream = CharStreams.fromString(content);
            var lexer = new SysVerilogHDLLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            _parser = new SysVerilogHDLParser(tokens) {BuildParseTree = true};
            Tree = _parser.module_declaration();
        }

        public void Show()
        {
            var s = Tree.ToStringTree(_parser);
            //var s = Tree.ToString(_parser);
            Console.WriteLine(s);
        }

        public override string ToString()
        {
            var visitor = new VerilogVisitor();
            var r = visitor.Visit(Tree);
            return r;
        }
    }
}
