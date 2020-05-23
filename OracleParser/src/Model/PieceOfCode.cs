using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class PieceOfCode
    {
        [JsonProperty]
        public int LineBeg { get; private set; }
        [JsonProperty]
        public int LineEnd { get; private set; }

        [JsonProperty]
        public int ColumnBeg { get; private set; }
        [JsonProperty]
        public int ColumnEnd { get; private set; }

        public void SetPosition(ParserRuleContext parser)
        {
            if (LineBeg == 0)
            {
                LineBeg = parser.Start.Line;
                ColumnBeg = parser.Start.Column;
            }

            LineEnd = parser.Stop.Line;
            ColumnEnd = parser.Stop.Column + parser.Stop.StopIndex - parser.Stop.StartIndex;
        }

        public void SetPosition(TerminalNodeImpl terminalNodeImpl)
        {
            if (LineBeg == 0)
            {
                LineBeg = terminalNodeImpl.Symbol.Line;
                ColumnBeg = terminalNodeImpl.Symbol.Column;
            }

            LineEnd = terminalNodeImpl.Symbol.Line;
            ColumnEnd = terminalNodeImpl.Symbol.Column + terminalNodeImpl.Symbol.StopIndex - terminalNodeImpl.Symbol.StartIndex;
        }

        public PieceOfCode()
        {

        }

        [JsonIgnore]
        public string PrintPos
        {
            get => $"({LineBeg}.{ColumnBeg}:{LineEnd}.{ColumnEnd})";
        }
    }
}
