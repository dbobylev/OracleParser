using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.src.Saver
{
    class BaseResult
    {
        public string Name { get; private set; }
        public string SHA { get; private set; }
        public DateTime Created { get; private set; }

        public BaseResult(string name, string sha)
        {
            Name = name;
            SHA = sha;
            Created = DateTime.Now;
        }
    }
}
