using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.src.Saver
{
    class BaseResult
    {
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public string SHA { get; private set; }
        [JsonProperty]
        public DateTime Created { get; private set; }

        public BaseResult(string name, string sha)
        {
            Name = name;
            SHA = sha;
            Created = DateTime.Now;
        }

        public BaseResult()
        {

        }
    }
}
