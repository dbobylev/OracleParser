using Newtonsoft.Json;
using OracleParser.src.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleParser.Model
{
    public class ParsedProcedure :PieceOfCode
    {
        [JsonProperty]
        private List<ParsedParameter> _parameters;
        [JsonProperty]
        private List<ParsedLink> _elements;

        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public PieceOfCode DeclarationPart { get; private set; }

        [JsonIgnore]
        public IReadOnlyCollection<ParsedParameter> Parameters { get => _parameters.AsReadOnly(); }
        [JsonIgnore]
        public IReadOnlyCollection<ParsedLink> Elements { get => _elements.AsReadOnly(); }

        public ParsedProcedure(string name)
        {
            Name = name;
            _parameters = new List<ParsedParameter>();
            _elements = new List<ParsedLink>();
        }

        public ParsedProcedure()
        {

        }

        public void AddParametr(ParsedParameter parameter)
        {
            _parameters.Add(parameter);
        }

        public void AddElement(ParsedLink element)
        {
            _elements.Add(element);
        }

        public void SetDeclarationPart(PieceOfCode decalrationPart)
        {
            DeclarationPart = decalrationPart;
        }

        public override string ToString()
        {
            return $"{Name}{PrintPos}: {string.Join(',', _parameters)}";
        }
    }
}
