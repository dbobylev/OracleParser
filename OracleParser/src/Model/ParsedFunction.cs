using Newtonsoft.Json;
using OracleParser.Model.PackageModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    [PackageElementType(ePackageElementType.Function)]
    public class ParsedFunction : ParsedProcedure
    {
        [JsonProperty]
        public string ReturnType { get; set; }

        public ParsedFunction(string name) : base(name)
        {

        }

        public ParsedFunction()
        {

        }
    }
}
