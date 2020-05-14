using OracleParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.src.Model
{
    public class Variable : PieceOfCode
    {
        public string Name { get; private set; }
        public string plType { get; private set; }

        public Variable(string name, string pltype)
        {
            Name = name;
            plType = pltype;
        }
    }
}
