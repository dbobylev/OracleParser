using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace OracleParser.Model.PackageModel
{
    public class PackageElement : IEquatable<ParsedProcedure>
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

        public bool Equals([AllowNull] ParsedProcedure other)
        {
            bool ans = Name.ToUpper() == other.Name.ToUpper();

            if (ans)
            {
                Func<List<ParsedParameter>, int> HashParam = 
                    (x) => x.Count == 0 ? 0 : 
                        x.Select((q, i) => (q.Name + i.ToString() + q.plType).ToUpper())
                         .Aggregate((a, b) => a + b)
                         .GetHashCode();
                ans = HashParam(Parametres) == HashParam(other.Parameters.ToList());
            }

            return ans;
        }
    }
}
