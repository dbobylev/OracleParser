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
        private List<ParsedElement> _elements;

        [JsonProperty]
        public string Name { get; private set; }

        [JsonIgnore]
        public IReadOnlyCollection<ParsedParameter> Parameters { get => _parameters.AsReadOnly(); }
        [JsonIgnore]
        public IReadOnlyCollection<ParsedElement> Elements { get => _elements.AsReadOnly(); }

        public ParsedProcedure(string name)
        {
            Name = name;
            _parameters = new List<ParsedParameter>();
            _elements = new List<ParsedElement>();
        }

        public ParsedProcedure()
        {

        }

        public void AddParametr(ParsedParameter parameter)
        {
            _parameters.Add(parameter);
        }

        public void AddElement(ParsedElement element)
        {
            _elements.Add(element);
        }

        public override string ToString()
        {
            return $"{Name}{PrintPos}: {string.Join(',', _parameters)}";
        }
    }
}
