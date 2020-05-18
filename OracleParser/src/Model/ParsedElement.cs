using Newtonsoft.Json;
using OracleParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class ParsedElement :PieceOfCode
    {
        [JsonProperty]
        public string Text { get; private set; }
        public ParsedElement(string text)
        {
            Text = text;
        }

        public ParsedElement()
        {

        }
    }
}
