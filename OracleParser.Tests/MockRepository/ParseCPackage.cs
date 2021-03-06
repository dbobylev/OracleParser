﻿using AntlrOraclePlsql;
using DataBaseRepository;
using DataBaseRepository.Model;
using Newtonsoft.Json;
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
        const string bodyPath = "C:\\TestRep\\ALPHA\\alpha.c_package.bdy";
        const string specPath = "C:\\TestRep\\ALPHA\\alpha.c_package.spc";

        [Test]
        public static void OraParserGetPackageTest()
        {
            DBRep.Instance().RepositoryPath = "C:\\TestRep\\";
            var x = OraParser.Instance().GetSavedPackage(new RepositoryPackage("c_package", "alpha"));
        }


        [Test]
        public static void RunTest()
        {
            var packagebody = OraParser.Instance().GetPackageBody(bodyPath);

            var json = JsonConvert.SerializeObject(packagebody, Formatting.Indented);
            Console.WriteLine(json);

            Assert.Pass();
        }

        [Test]
        public static void PrintSpec()
        {
            var x = Analyzer.RunUpperCase(specPath);

            PrintRuleTree.PrintChilds(x);

            Assert.Pass();
        }

        [Test]
        public static void PrintBody()
        {
            var x = Analyzer.RunUpperCase(bodyPath);

            PrintRuleTree.PrintChilds(x);

            Assert.Pass();
        }
    }
}
