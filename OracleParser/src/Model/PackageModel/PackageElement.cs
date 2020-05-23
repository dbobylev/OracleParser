using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleParser.Model.PackageModel
{
    class PackageElement
    {
        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public ePackageElementType ElementType { get; private set; }

        [JsonProperty]
        public Dictionary<ePackageElementDefinitionType, PieceOfCode> Position { get; private set; }

        [JsonProperty]
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

        public void AddLinks(IEnumerable<ParsedLink> links)
        {
            Links.AddRange(links);
        }
    }
}
