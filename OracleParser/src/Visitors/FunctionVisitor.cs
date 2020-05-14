using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using AntlrOraclePlsql;
using OracleParser.Model;
using OracleParser.src.Model;
using OracleParser.src.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleParser.Visitors
{
    class FunctionVisitor : PlSqlParserBaseVisitor<Function>
    {
        private ParameterVisitor _parameterVisitor = new ParameterVisitor();
        private Function _Result;

        protected override Function DefaultResult => _Result;

        public override Function Visit(IParseTree tree)
        {
            var identifierContext = tree.GetChild(1);
            if (!(identifierContext is PlSqlParser.IdentifierContext))
                throw new NotImplementedException("Ожидалось имя функции");
            _Result = new Function(identifierContext.GetText());
            return base.Visit(tree);
        }

        public override Function VisitType_spec([NotNull] PlSqlParser.Type_specContext context)
        {
            _Result.SetReturnType(context.GetText());
            return base.VisitType_spec(context);
        }

        public override Function VisitParameter([NotNull] PlSqlParser.ParameterContext context)
        {
            var parameter = _parameterVisitor.Visit(context).SetPositionExt(context);
            _Result.AddParametr(parameter);
            return base.VisitParameter(context);
        }

        public override Function VisitGeneral_element_part([NotNull] PlSqlParser.General_element_partContext context)
        {
            _Result.AddElement(Helper.ReadElement(context));
            return base.VisitGeneral_element_part(context);
        }
    }
}
