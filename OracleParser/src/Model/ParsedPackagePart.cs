using Newtonsoft.Json;
using OracleParser.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace OracleParser.Model
{
    public class ParsedPackagePart
    {
        [JsonProperty]
        public List<ParsedProcedure> Procedures;
        [JsonProperty]
        public List<ParsedVariable> Variables;
        [JsonProperty]
        public List<ParsedObject> Objects;

        public ParsedPackagePart()
        {
            Procedures = new List<ParsedProcedure>();
            Variables = new List<ParsedVariable>();
            Objects = new List<ParsedObject>();
        }
    }
}
