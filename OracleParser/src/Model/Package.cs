using OracleParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class Package
    {
        public PackagePart Spec { get; private set; }
        public PackagePart Body { get; private set; }

        public Package(PackagePart spec, PackagePart body)
        {
            Spec = spec;
            Body = body;
        }
    }
}
