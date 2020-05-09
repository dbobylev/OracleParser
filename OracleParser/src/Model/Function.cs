using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    public class Function : Procedure
    {
        public string ReturnType { get; private set; }
        public Function(string name):base(name)
        {

        }

        public void SetReturnType(string pltype)
        {
            ReturnType = pltype;
        }

        public override string ToString()
        {
            return $"{base.ToString()} return: {ReturnType}";
        }
    }
}
