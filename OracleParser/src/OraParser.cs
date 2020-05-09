using Microsoft.Extensions.Configuration;
using Serilog;
using OracleParser.Model;
using AntlrOraclePlsql;
using Antlr4.Runtime.Tree;
using OracleParser.Visitors;

namespace OracleParser
{
    public class OraParser
    {
        public OraParser()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Seri.InitConfig(configuration);
            Seri.Log.Information("Hello, world! Serilog loaded!");
        }

        public PackageBody GetPackageBody(string filePath)
        {
            IParseTree tree = Analyzer.analyze(filePath);
            PackageBodyVisitor visitor = new PackageBodyVisitor();
            PackageBody packageBody = visitor.Visit(tree);
            return packageBody;
        }
    }
}
