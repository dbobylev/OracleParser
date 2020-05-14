using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DataBaseRepository;
using DataBaseRepository.Model;
using NUnit.Framework;

namespace OracleParser.Tests
{
    class DBRepTest
    {
        [Test(Description ="Ручной запуск для дебага")]
        public static void ReadFileLines()
        {
            RepositoryObject obj = new RepositoryObject("text13k", "source", eRepositoryObjectType.Text);
            DBRep rep = DBRep.Instance();
            rep.RepositoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string s = DBRep.Instance().GetTextOfFile(obj, 10, 18);
            Console.WriteLine(s);

            Assert.Pass();
        }

        [Test]
        public static void GetOwnersTest()
        {
            string TempRep = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "source", "TempRep");
            Directory.CreateDirectory(TempRep);
            Directory.CreateDirectory(Path.Combine(TempRep, "Alpha"));
            Directory.CreateDirectory(Path.Combine(TempRep, "Beta"));
            Directory.CreateDirectory(Path.Combine(TempRep, "Gamma"));
            Directory.CreateDirectory(Path.Combine(TempRep, "Gamma", "Delta"));
            File.WriteAllText(Path.Combine(TempRep, "Alpha", "temp1.txt"), "test");
            File.WriteAllText(Path.Combine(TempRep, "Gamma", "temp2.txt"), "test");
            File.WriteAllText(Path.Combine(TempRep, "Gamma", "Delta", "temp3.txt"), "test");

            DBRep rep = DBRep.Instance();
            rep.RepositoryPath = TempRep;
            string[] z = rep.GetOwners().ToArray();
            Directory.Delete(TempRep, true);
            Assert.AreEqual(new string[] { "ALPHA", "BETA", "GAMMA" }, z);
        }
    }
}
