using Newtonsoft.Json;
using OracleParser.Model;
using OracleParser.Model.PackageModel;
using OracleParser.Saver;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Saver
{
    class PackageResult : BaseResult
    {
        [JsonProperty]
        public Package _package { get; private set; }

        public PackageResult(Package package, string name): base(name)
        {
            _package = package;
        }

        public PackageResult()
        {

        }
    }
}
