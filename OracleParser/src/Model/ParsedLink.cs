using Newtonsoft.Json;
using OracleParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class ParsedLink :PieceOfCode
    {
        [JsonProperty]
        public string Text { get; private set; }
        public ParsedLink(string text)
        {
            Text = text;
        }

        public ParsedLink()
        {

        }
    }
}
