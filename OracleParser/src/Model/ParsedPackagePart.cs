using Newtonsoft.Json;
using OracleParser.src.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace OracleParser.Model
{
    public class ParsedPackagePart
    {
        [JsonProperty]
        private List<ParsedProcedure> _procedures;
        [JsonProperty]
        private List<ParsedVariable> _variables;

        [JsonIgnore]
        public IReadOnlyList<ParsedProcedure> Procedures { get => _procedures.AsReadOnly(); }
        [JsonIgnore]
        public IReadOnlyList<ParsedVariable> Variables { get => _variables.AsReadOnly(); }

        public ParsedPackagePart()
        {
            _procedures = new List<ParsedProcedure>();
            _variables = new List<ParsedVariable>();
        }

        public void AddProcedure(ParsedProcedure procedure)
        {
            _procedures.Add(procedure);
        }

        public void AddVariable(ParsedVariable variable)
        {
            _variables.Add(variable);
        }
    }
}
