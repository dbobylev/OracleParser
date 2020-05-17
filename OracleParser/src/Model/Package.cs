using OracleParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class Package
    {
        public string Name { get; private set; }

        public PackagePart Spec { get; private set; }
        public PackagePart Body { get; private set; }

        public Package(string name, PackagePart spec, PackagePart body)
        {
            Name = name;
            Spec = spec;
            Body = body;
        }
    }
}
