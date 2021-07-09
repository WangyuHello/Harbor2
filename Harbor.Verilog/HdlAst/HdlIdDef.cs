namespace Harbor.Verilog.HdlAst
{
    public class HdlIdDef: WithNameAndDoc, iHdlObj
    {
        public iHdlExprItem type;
        public iHdlExprItem value;
        public bool is_latched;
        public bool is_const;
        public bool is_static;
        public bool is_shared;
        public HdlDirection direction;

        public HdlIdDef(string id, iHdlExprItem _type, iHdlExprItem _val): base(id)
        {
            type = _type;
            value = _val;
            direction = HdlDirection.DIR_INTERNAL;
        }

        public HdlIdDef(string id, iHdlExprItem _type, iHdlExprItem _val, HdlDirection _direction, bool _is_latched): this(id, _type, _val)
        {
            direction = _direction;
            is_latched = _is_latched;
        }
    }
}
