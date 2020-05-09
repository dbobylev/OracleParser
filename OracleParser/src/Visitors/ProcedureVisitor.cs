using AntlrOraclePlsql;
using System;
using System.Collections.Generic;
using System.Text;
using OracleParser.Model;
using OracleParser.Visitors;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Misc;

namespace OracleParser.src.Visitors
{
    class ProcedureVisitor:PlSqlParserBaseVisitor<Model.Procedure>
    {
        private ParameterVisitor _parameterVisitor = new ParameterVisitor();
        protected Procedure _Result;

        protected override Procedure DefaultResult => _Result;

        public override Procedure Visit(IParseTree tree)
        {
            var identifierContext = tree.GetChild(1);
            if (!(identifierContext is PlSqlParser.IdentifierContext))
                throw new NotImplementedException("Ожидалось имя процедуры");
            _Result = new Procedure(identifierContext.GetText());
            return base.Visit(tree);
        }

        public override Procedure VisitParameter([NotNull] PlSqlParser.ParameterContext context)
        {
            Seri.Log.Verbose($"Visit: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            var parameter = _parameterVisitor.Visit(context).SetPositionExt(context);
            _Result.AddParametr(parameter);
            return base.VisitParameter(context);
        }
    }
}
