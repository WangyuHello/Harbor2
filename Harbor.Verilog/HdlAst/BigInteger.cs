// ReSharper disable InconsistentNaming
namespace Harbor.Verilog.HdlAst
{
    public class BigInteger
    {
        public const int INVALID_BASE = -1;
        public const int BIN_BASE = 2;
        public const int OCT_BASE = 8;
        public const int DEC_BASE = 10;
        public const int HEX_BASE = 16;
        public const int CHAR_BASE = 256;

        public long val; //int64
        public string bitstring;
        public int bitstring_base;

        public BigInteger(long v)
        {
            val = v;
            bitstring_base = INVALID_BASE;
        }

        public BigInteger(string _bit_string, int _base)
        {
            val = 0;
            bitstring = _bit_string;
            bitstring_base = _base;
        }

        public bool is_bitstring()
        {
            return bitstring_base != INVALID_BASE;
        }
    }
}
