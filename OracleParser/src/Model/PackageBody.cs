using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace OracleParser.Model
{
    public class PackageBody
    {
        private List<Procedure> _procedures;

        public IReadOnlyCollection<Procedure> Procedures { get => _procedures.AsReadOnly(); }

        public PackageBody()
        {
            _procedures = new List<Procedure>();
        }

        public void AddProcedure(Procedure procedure)
        {
            _procedures.Add(procedure);
        }
    }
}
