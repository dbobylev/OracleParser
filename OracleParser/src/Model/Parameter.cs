using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class Parameter :PieceOfCode
    {
        public string Name { get; private set; }
        public string plType { get; private set; }

        public Parameter(string name)
        {
            Name = name;
        }

        public void SetType(string pltype)
        {
            plType = pltype;
        }

        public override string ToString()
        {
            return $"{Name} {plType}{PrintPos}";
        }
    }
}
