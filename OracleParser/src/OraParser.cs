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
            Func<string, PackagePart> GetPart = (x) =>
            {
                var path = Path.Combine(DBRep.Instance().RepositoryPath, x);
                var visitor = new PackageBodyVisitor();
                var tree = Analyzer.RunUpperCase(path);
                var packagePart = visitor.Visit(tree);
                return packagePart;
            };

            var spec = GetPart(repPackage.SpecRepFilePath);
            var body = GetPart(repPackage.BodyRepFilePath);
            var package = new Package(spec, body);

            return package;
        }
    }
}
