using Newtonsoft.Json;
using OracleParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.src.Model
{
    public class Element :PieceOfCode
    {
        [JsonProperty]
        public string Text { get; private set; }
        public Element(string text)
        {
            Text = text;
        }

        public Element()
        {

        }
    }
}
