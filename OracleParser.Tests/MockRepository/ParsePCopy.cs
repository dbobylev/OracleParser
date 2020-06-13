using AntlrOraclePlsql;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Tests.MockRepository
{
    class ParsePCopy
    {
        private const string BodyPath = "C:\\TestRep\\ALPHA\\ALPHA.pcopy.bdy";

        [Test]
        public static void PrintBody()
        {
            var x = Analyzer.RunUpperCase(BodyPath);

            PrintRuleTree.PrintChilds(x);

            Assert.Pass();
        }
    }
}
