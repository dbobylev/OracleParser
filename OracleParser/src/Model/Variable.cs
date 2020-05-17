using Newtonsoft.Json;
using OracleParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.src.Model
{
    public class Variable : PieceOfCode
    {
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public string plType { get; private set; }

        public Variable(string name, string pltype)
        {
            Name = name;
            plType = pltype;
        }

        public Variable()
        {

        }
    }
}
