using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleParser.Model.PackageModel
{
    public class PackageElement
    {
        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public ePackageElementType ElementType { get; private set; }

        [JsonProperty]
        public Dictionary<ePackageElementDefinitionType, PieceOfCode> Position { get; private set; } = new Dictionary<ePackageElementDefinitionType, PieceOfCode>();

        [JsonProperty]
        public List<ParsedLink> Links { get; private set; } = new List<ParsedLink>();

        public List<ParsedParameter> Parametres { get; private set; } = new List<ParsedParameter>();

        [JsonIgnore]
        public bool HasSpec => Position.Keys.Contains(ePackageElementDefinitionType.Spec);
        [JsonIgnore]
        public bool HasBody => Position.Keys.Contains(ePackageElementDefinitionType.BodyFull);

        public PackageElement(string name, ePackageElementType elementType)
        {
            Name = name;
            ElementType = elementType;
        }

        public PackageElement()
        {

        }

        public void AddPosition(ePackageElementDefinitionType packageElementDefinitionType, PieceOfCode posCode)
        {
            Position.Add(packageElementDefinitionType, posCode);
        }
    }
}
