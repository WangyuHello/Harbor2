using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace Harbor.Verilog.HdlAst
{
    /// <summary>
    /// Container for position in code.
    /// NOTE: stopXX are inclusive coordinates and not one beyond i.e. [startXXX, stopXXX] and not [startXXX, stopXXX)
    /// Also, coordinates are 1-based indexing i.e. first line and column is indexed as 1 and not 0.
    /// </summary>
    public class CodePosition
    {
        public static int INVALID = int.MaxValue;

        public int start_line;
        public int stop_line;
        public int start_column;
        public int stop_column;

        public CodePosition() : this(INVALID, INVALID, INVALID, INVALID)
        {

        }

        public CodePosition(int startLine, int stopLine, int startColumn, int stopColumn)
        {
            start_line = startLine;
            stop_line = stopLine;
            start_column = startColumn;
            stop_column = stopColumn;
        }

        /// <summary>
        /// template class ELEM_T
        /// void update_from_elem(ELEM_T *elem)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elem"></param>
        public void update_from_elem<T>(T elem)
        {
            //start_line = elem->getStart()->getLine();
            //stop_line = elem->getStop()->getLine();
            //start_column = elem->getStart()->getCharPositionInLine() + 1;
            //stop_column = elem->getStop()->getCharPositionInLine() +
            //              (elem->getStop()->getStopIndex() - elem->getStop()->getStartIndex()) + 1;
        }

        public bool isKnown()
        {
            return start_line != INVALID || stop_line != INVALID || start_column != INVALID || stop_column != INVALID;
        }
    }
}
