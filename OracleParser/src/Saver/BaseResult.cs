using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Saver
{
    class BaseResult
    {
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public DateTime Created { get; private set; }

        public BaseResult(string name)
        {
            Name = name;
            Created = DateTime.Now;
        }

        public BaseResult()
        {

        }
    }
}
