namespace Harbor.Verilog.HdlAst
{
    public class Named
    {
        public string name { get; set; }
        
        public Named() { }
        public Named(string name)
        {
            this.name = name;
        }
    }

    public interface WithDoc
    {
        public string __doc__ { get; set; }
    }

    public interface WithPos
    {
        public CodePosition position { get; set; }
    }

    public class WithNameAndDoc : Named, WithDoc, WithPos
    {
        public string __doc__ { get; set; }
        public CodePosition position { get; set; }

        public WithNameAndDoc() { }
        public WithNameAndDoc(string name) : base(name) { }
    }
}
