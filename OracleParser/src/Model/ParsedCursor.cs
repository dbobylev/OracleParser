using OracleParser.Model.PackageModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    [PackageElementType(ePackageElementType.Cursor)]
    class ParsedCursor: ParsedObject
    {
        public ParsedCursor(string name) :base(name)
        {

        }
    }
}