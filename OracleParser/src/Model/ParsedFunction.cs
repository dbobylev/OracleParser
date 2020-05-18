using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class ParsedFunction : ParsedProcedure
    {
        [JsonProperty]
        public string ReturnType { get; private set; }
        public ParsedFunction(string name):base(name)
        {

        }

        public ParsedFunction()
        {

        }

        public void SetReturnType(string pltype)
        {
            ReturnType = pltype;
        }

        public override string ToString()
        {
            return $"{base.ToString()} return: {ReturnType}";
        }
    }
}
