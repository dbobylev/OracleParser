using Newtonsoft.Json;
using OracleParser.Model;
using OracleParser.Model.PackageModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    [PackageElementType(ePackageElementType.Variable)]
    public class ParsedVariable : ParsedObject
    {
        [JsonProperty]
        public string plType { get; private set; }

        public ParsedVariable(string name, string pltype) :base(name)
        {
            plType = pltype;
        }

        public ParsedVariable()
        {

        }
    }
}
