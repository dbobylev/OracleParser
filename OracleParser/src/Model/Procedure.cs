using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class Procedure :PieceOfCode
    {
        private List<Parameter> _parameters;

        public string Name { get; private set; }
        public IReadOnlyCollection<Parameter> Parameters { get => _parameters.AsReadOnly(); }

        public Procedure(string name)
        {
            Name = name;
            _parameters = new List<Parameter>();
        }

        public void AddParametr(Parameter parameter)
        {
            _parameters.Add(parameter);
        }

        public override string ToString()
        {
            return $"{Name}{PrintPos}: {string.Join(',', _parameters)}";
        }
    }
}
