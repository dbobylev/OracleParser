using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class ParsedParameter :PieceOfCode
    {
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string plType { get; private set; }
        [JsonProperty]
        public PieceOfCode NamePart { get; set; }

        public ParsedParameter(string name)
        {
            Name = name;
            NamePart = new PieceOfCode();
        }

        public ParsedParameter()
        {

        }

        public void SetType(string pltype)
        {
            plType = pltype;
        }

        public override string ToString()
        {
            return $"{Name} {plType}{PrintPos}";
        }
    }
}
