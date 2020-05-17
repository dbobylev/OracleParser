using Antlr4.Runtime;
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
