using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class Parameter :PieceOfCode
    {
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public string plType { get; private set; }

        public Parameter(string name)
        {
            Name = name;
        }

        public Parameter()
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
