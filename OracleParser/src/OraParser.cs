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
using System.Threading.Tasks;

namespace OracleParser
{
    public class OraParser
    {
        public event Action<eRepositoryObjectType> ObjectWasParsed;

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

        public async Task<Package> GetPackage(RepositoryPackage repPackage, bool ForseParse = false)
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

                var spec = await GetPart(repPackage.SpecRepFullPath);
                ObjectWasParsed?.Invoke(eRepositoryObjectType.Package_Spec);
                var body = await GetPart(repPackage.BodyRepFullPath);
                ObjectWasParsed?.Invoke(eRepositoryObjectType.Package_Body);

                answer = new Package(spec, body, repPackage);
                manager.SaveParsedPackage(repPackage, answer);
            }

            ObjectWasParsed = null;
            return answer;
        }

        private async Task<ParsedPackagePart> GetPart(string path)
        {
           return await Task.Run(() =>
           {
               var visitor = new PackageBodyVisitor();
               var tree = Analyzer.RunUpperCase(path);
               if (tree.exception != null)
                   throw tree.exception;
               var packagePart = visitor.Visit(tree);
               return packagePart;
           });
        }
    }
}
