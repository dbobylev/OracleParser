using Newtonsoft.Json;
using NUnit.Framework;
using AntlrOraclePlsql;
using OracleParser.Tests.Source;
using System;
using System.IO;

namespace OracleParser.Tests
{
    public class OraclePArserApiTest
    {
        [Test]
        public static void GetPackageTest()
        {
            OraParser oracleParser = OraParser.Instance();
            var packagebody = oracleParser.GetPackageBody(SourceFiles.pathPackageTest);
            var json = JsonConvert.SerializeObject(packagebody, Formatting.Indented);
            Console.WriteLine(json);
            Assert.Pass();
        }
    }
}