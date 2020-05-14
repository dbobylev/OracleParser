using AntlrOraclePlsql;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OracleParser.Tests.MockRepository
{
    class ParseCPackage
    {
        const string bodyPath = "C:\\TestRep\\ALPHA\\c_package.bdy";

        [Test]
        public static void RunTest()
        {
            var z = Analyzer.RunUpperCase(bodyPath);

            Assert.Pass();
        }
    }
}
