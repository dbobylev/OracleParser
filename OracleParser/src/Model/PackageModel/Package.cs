using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model.PackageModel
{
    class Package
    {
        List<PackageElement> elements;

        public Package(ParsedPackagePart spec, ParsedPackagePart body)
        {
            elements = new List<PackageElement>();

            HashSet<string> ComplitedElements = new HashSet<string>();

            for (int i = 0; i < spec.Procedures.Count; i++)
            {
            }
        }
    }
}
