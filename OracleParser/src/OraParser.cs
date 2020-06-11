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
using OracleParser.Saver;
using OracleParser.Model.PackageModel;

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

        public Package GetPackage(RepositoryPackage repPackage, bool ForseParse = false)
        {
            Seri.Log.Debug($"Начинаем GetPackage, repPackage={repPackage}");
            Package answer;

            PackageManager manager = new PackageManager();

            if (!ForseParse && manager.CheckPackage(repPackage, out Package savedParderPackage))
            {
                Seri.Log.Debug("Пресохраненные данные найдены, возвращаем их");
                answer = savedParderPackage;
            }
            else
            {
                Seri.Log.Debug("Сохраненный данные не найдены");
                Seri.Log.Information($"Запускаем парсинг объекта, repPackage={repPackage}");

                Func<string, ParsedPackagePart> GetPart = (x) =>
                {
                    var visitor = new PackageBodyVisitor();
                    var tree = Analyzer.RunUpperCase(x);
                    if (tree.exception != null)
                        throw tree.exception;
                    var packagePart = visitor.Visit(tree);
                    return packagePart;
                };

                var spec = GetPart(repPackage.SpecRepFullPath);
                var body = GetPart(repPackage.BodyRepFullPath);

                answer = new Package(spec, body, repPackage);
                manager.SaveParsedPackage(repPackage, answer);
            }

            return answer;
        }
    }
}
