using Newtonsoft.Json;
using OracleParser.src.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace OracleParser.Model
{
    public class PackagePart
    {
        [JsonProperty]
        private List<Procedure> _procedures;
        [JsonProperty]
        private List<Variable> _variables;

        [JsonIgnore]
        public IReadOnlyList<Procedure> Procedures { get => _procedures.AsReadOnly(); }
        [JsonIgnore]
        public IReadOnlyList<Variable> Variables { get => _variables.AsReadOnly(); }

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
