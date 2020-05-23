using Newtonsoft.Json;
using OracleParser.Model;
using OracleParser.Model.PackageModel;
using OracleParser.src.Saver;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Saver
{
    class PackageResult : BaseResult
    {
        [JsonProperty]
        public Package _package { get; private set; }

        public PackageResult(Package package, string name, string sha): base(name, sha)
        {
            _package = package;
        }

        public PackageResult()
        {

        }
    }
}
