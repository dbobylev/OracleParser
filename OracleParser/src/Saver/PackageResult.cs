using OracleParser.Model;
using OracleParser.src.Saver;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Saver
{
    class PackageResult : BaseResult
    {
        public Package _package { get; private set; }

        public PackageResult(Package package, string sha): base(package.Name, sha)
        {
            _package = package;
        }
    }
}
