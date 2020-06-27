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
    public class OraParser :IOraParser
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

        public Package GetSavedPackage(RepositoryPackage repositoryPackage)
        {
            Seri.Log.Debug($"Начинаем GetSavedPackage, repPackage={repositoryPackage}");
            Package answer = null;

            var manager = new PackageManager();

            if (manager.CheckPackage(repositoryPackage, out Package savedParderPackage))
            {
                Seri.Log.Debug("Пресохраненные данные найдены, возвращаем их");
                answer = savedParderPackage;
            }

            return answer;
        }

        public async Task<Package> GetPackage(RepositoryPackage repositoryPackage, bool allowNationalChars)
        {
            return GetSavedPackage(repositoryPackage) ?? await ParsePackage(repositoryPackage, allowNationalChars);
        }

        public async Task<Package> ParsePackage(RepositoryPackage repositoryPackage, bool allowNationalChars)
        {
            Seri.Log.Information($"Запускаем парсинг объекта, repPackage={repositoryPackage}");

            var spec = await GetPart(repositoryPackage.SpecRepFullPath, allowNationalChars);
            ObjectWasParsed?.Invoke(eRepositoryObjectType.Package_Spec);
            var body = await GetPart(repositoryPackage.BodyRepFullPath, allowNationalChars);
            ObjectWasParsed?.Invoke(eRepositoryObjectType.Package_Body);

            var answer = new Package(spec, body, repositoryPackage);
            var manager = new PackageManager();
            manager.SaveParsedPackage(repositoryPackage, answer);

            ObjectWasParsed = null;
            return answer;
        }

        private async Task<ParsedPackagePart> GetPart(string path, bool allowNationalChars)
        {
           return await Task.Run(() =>
           {
               var visitor = new PackageBodyVisitor();
               var tree = Analyzer.RunUpperCase(path, allowNationalChars);
               if (tree.exception != null)
                   throw tree.exception;
               var packagePart = visitor.Visit(tree);
               return packagePart;
           });
        }
    }
}
