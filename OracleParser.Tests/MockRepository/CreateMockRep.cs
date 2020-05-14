using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OracleParser.Tests.MockRepository
{
    class CreateMockRep
    {
        [Test]
        public static void CreateMockRepTest()
        {
            Directory.CreateDirectory("C:\\TestRep");
            Directory.CreateDirectory("C:\\TestRep\\ALPHA");
            Directory.CreateDirectory("C:\\TestRep\\BETA");
            File.WriteAllText("C:\\TestRep\\ALPHA\\ALPHA.c_package.spc", TextCPackage.Spec);
            File.WriteAllText("C:\\TestRep\\ALPHA\\ALPHA.c_package.bdy", TextCPackage.Body);

            Assert.Pass();
        }
    }
}
