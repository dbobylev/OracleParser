using Newtonsoft.Json;
using OracleParser.src.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleParser.Model
{
    public class Procedure :PieceOfCode
    {
        [JsonProperty]
        private List<Parameter> _parameters;
        [JsonProperty]
        private List<Element> _elements;

        [JsonProperty]
        public string Name { get; private set; }

        [JsonIgnore]
        public IReadOnlyCollection<Parameter> Parameters { get => _parameters.AsReadOnly(); }
        [JsonIgnore]
        public IReadOnlyCollection<Element> Elements { get => _elements.AsReadOnly(); }

        public Procedure(string name)
        {
            Name = name;
            _parameters = new List<Parameter>();
            _elements = new List<Element>();
        }

        public Procedure()
        {

        }

        public void AddParametr(Parameter parameter)
        {
            _parameters.Add(parameter);
        }

        public void AddElement(Element element)
        {
            _elements.Add(element);
        }

        public override string ToString()
        {
            return $"{Name}{PrintPos}: {string.Join(',', _parameters)}";
        }
    }
}
