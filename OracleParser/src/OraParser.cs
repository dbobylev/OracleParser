using Microsoft.Extensions.Configuration;
using Serilog;
using OracleParser.Model;
using AntlrOraclePlsql;
using Antlr4.Runtime.Tree;
using OracleParser.Visitors;
using DataBaseRepository.Model;
using System;
using DataBaseRepository;
using System.IO;
using OracleParser.src.Saver;

namespace OracleParser
{
    public class OraParser
    {
        private static OraParser _instance;
        public static OraParser Instance()
        {
            if (_instance == null)
                _instance = new OraParser();
            return _instance;
        }

        private OraParser()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Seri.InitConfig(configuration);
            Seri.Log.Information("Hello, world! Serilog loaded!");
        }

        public PackagePart GetPackageBody(string filePath)
        {
            IParseTree tree = Analyzer.RunUpperCase(filePath);
            PackageBodyVisitor visitor = new PackageBodyVisitor();
            PackagePart packageBody = visitor.Visit(tree);
            return packageBody;
        }

        public Package GetPackage(RepositoryPackage repPackage)
        {
            Package answer;

            Func<string, PackagePart> GetPart = (x) =>
            {
                var visitor = new PackageBodyVisitor();
                var tree = Analyzer.RunUpperCase(x);
                var packagePart = visitor.Visit(tree);
                return packagePart;
            };

            PackageManager manager = new PackageManager();

            if (manager.CheckParsedPackage(repPackage, out Package savedParderPackage))
            {
                Seri.Log.Verbose("CehckedParsed true");
                answer = savedParderPackage;
            }
            else
            {
                Seri.Log.Verbose("CheckedParsed false");

                var spec = GetPart(repPackage.SpecRepFullPath);
                var body = GetPart(repPackage.BodyRepFullPath);

                answer = new Package(repPackage.ObjectName, spec, body);
                manager.SaveParsedPackage(repPackage, answer);
            }

            return answer;
        }
    }
}
