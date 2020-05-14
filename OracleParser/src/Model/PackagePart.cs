using OracleParser.src.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace OracleParser.Model
{
    public class PackagePart
    {
        private List<Procedure> _procedures;
        private List<Variable> _variables;

        public IReadOnlyCollection<Procedure> Procedures { get => _procedures.AsReadOnly(); }
        public IReadOnlyCollection<Variable> Variables { get => _variables.AsReadOnly(); }

        public PackagePart()
        {
            _procedures = new List<Procedure>();
            _variables = new List<Variable>();
        }

        public void AddProcedure(Procedure procedure)
        {
            _procedures.Add(procedure);
        }

        public void AddVariable(Variable variable)
        {
            _variables.Add(variable);
        }
    }
}
