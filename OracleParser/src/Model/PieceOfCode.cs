using Antlr4.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class PieceOfCode
    {
        public int LineBeg { get; private set; }
        public int LineEnd { get; private set; }

        public int ColumnBeg { get; private set; }
        public int ColumnEnd { get; private set; }

        public void SetPosition(ParserRuleContext parser)
        {
            LineBeg = parser.Start.Line;
            LineEnd = parser.Stop.Line;

            ColumnBeg = parser.Start.Column;
            ColumnEnd = parser.Stop.Column + parser.Stop.StopIndex - parser.Stop.StartIndex;
        }

        [JsonIgnore]
        public string PrintPos
        {
            get => $"({LineBeg}.{ColumnBeg}:{LineEnd}.{ColumnEnd})";
        }
    }
}
