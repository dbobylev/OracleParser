using Newtonsoft.Json;
using NUnit.Framework;
using AntlrOraclePlsql;
using OracleParser.Tests.Source;
using System;
using System.IO;
using DataBaseRepository.Model;
using DataBaseRepository;

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

        [Test]
        public static void GetPackageTest2()
        {
            OraParser oracleParser = OraParser.Instance();

            DBRep.Instance().RepositoryPath = "C:\\TestRep";
            RepositoryPackage rep = new RepositoryPackage("c_package", "alpha");

            var z = oracleParser.GetPackage(rep);
        }
    }
}