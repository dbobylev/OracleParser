using System;
using System.Collections.Generic;
using System.Text;
using OracleParser.Model.PackageModel;

namespace OracleParser.Model
{
    [PackageElementType(ePackageElementType.Type)]
    public class ParsedType :ParsedObject
    {
        public ParsedType(string name) :base(name)
        {

        }
    }
}
