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
            var configuration = new ConfigurationBuilder().AddJsonFile("OracleParser.json").Build();
            Seri.InitConfig(configuration);
        }

        public ParsedPackagePart GetPackageBody(string filePath)
        {
            IParseTree tree = Analyzer.RunUpperCase(filePath);
            PackageBodyVisitor visitor = new PackageBodyVisitor();
            ParsedPackagePart packageBody = visitor.Visit(tree);
            return packageBody;
        }

        public ParsedPackage GetPackage(RepositoryPackage repPackage)
        {
            Seri.Log.Debug($"Начинаем GetPackage, repPackage={repPackage}");
            ParsedPackage answer;

            Func<string, ParsedPackagePart> GetPart = (x) =>
            {
                var visitor = new PackageBodyVisitor();
                var tree = Analyzer.RunUpperCase(x);
                var packagePart = visitor.Visit(tree);
                return packagePart;
            };

            PackageManager manager = new PackageManager();

            if (manager.CheckParsedPackage(repPackage, out ParsedPackage savedParderPackage))
            {
                Seri.Log.Debug("Пресохраненные данные найдены, возвращаем их");
                answer = savedParderPackage;
            }
            else
            {
                Seri.Log.Debug("Сохраненный данные не найдены");
                Seri.Log.Information($"Запускаем парсинг объекта, repPackage={repPackage}");

                var spec = GetPart(repPackage.SpecRepFullPath);
                var body = GetPart(repPackage.BodyRepFullPath);

                answer = new ParsedPackage(repPackage.ObjectName, spec, body);
                manager.SaveParsedPackage(repPackage, answer);
            }

            return answer;
        }
    }
}
