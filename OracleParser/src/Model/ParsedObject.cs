using Newtonsoft.Json;
using OracleParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public abstract class ParsedObject : PieceOfCode
    {
        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public PieceOfCode NameIdentifierPart { get; set; }

        public ParsedObject(string name)
        {
            Name = name;
            NameIdentifierPart = new PieceOfCode();
        }
    }
}
