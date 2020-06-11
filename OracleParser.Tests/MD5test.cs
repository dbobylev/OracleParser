using NUnit.Framework;
using OracleParser.Saver;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Tests
{
    class MD5test
    {
        [Test]
        public static void MD5Test()
        {
            string s1 = MD5Utils.CalculateMD5path("C:\\TestRep\\ALPHA\\ALPHA.c_package.bdy");
            Console.WriteLine(s1);

            string s2 = MD5Utils.CalculateMD5path("C:\\TestRep\\ALPHA\\ALPHA.c_package.spc");
            Console.WriteLine(s2);
        }
    }
}
