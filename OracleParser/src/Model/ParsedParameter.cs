using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class ParsedParameter :PieceOfCode
    {
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public string plType { get; private set; }

        public ParsedParameter(string name)
        {
            Name = name;
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
