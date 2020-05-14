using OracleParser.src.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleParser.Model
{
    public class Procedure :PieceOfCode
    {
        private List<Parameter> _parameters;
        private List<Element> _elements;

        public string Name { get; private set; }
        public IReadOnlyCollection<Parameter> Parameters { get => _parameters.AsReadOnly(); }
        public IReadOnlyCollection<Element> Elements { get => _elements.AsReadOnly(); }

        public Procedure(string name)
        {
            Name = name;
            _parameters = new List<Parameter>();
            _elements = new List<Element>();
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
