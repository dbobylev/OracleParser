using Newtonsoft.Json;
using OracleParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class ParsedPackage
    {
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public ParsedPackagePart Spec { get; private set; }
        [JsonProperty]
        public ParsedPackagePart Body { get; private set; }

        public ParsedPackage(string name, ParsedPackagePart spec, ParsedPackagePart body)
        {
            Name = name;
            Spec = spec;
            Body = body;
        }

        public ParsedPackage()
        {

        }
    }
}
