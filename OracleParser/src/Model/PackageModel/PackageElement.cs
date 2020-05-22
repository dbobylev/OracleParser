using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleParser.Model.PackageModel
{
    class PackageElement
    {
        public string Name { get; private set; }
        public ePackageElementType ElementType { get; private set; }
        public Dictionary<ePackageElementDefinitionType, PieceOfCode> Position { get; private set; }
        public List<ParsedLink> Links { get; private set; }

        public PackageElement(string name, ePackageElementType elementType)
        {
            Name = name;
            ElementType = elementType;
            Position = new Dictionary<ePackageElementDefinitionType, PieceOfCode>();
            Links = new List<ParsedLink>();
        }

        public void AddPosition(ePackageElementDefinitionType packageElementDefinitionType, PieceOfCode posCode)
        {
            Position.Add(packageElementDefinitionType, posCode);
        }

        public void AddLink(ParsedLink link)
        {
            Links.Add(link);
        }
    }
}
