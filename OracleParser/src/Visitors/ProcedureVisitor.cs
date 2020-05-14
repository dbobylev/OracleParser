using AntlrOraclePlsql;
using System;
using System.Collections.Generic;
using System.Text;
using OracleParser.Model;
using OracleParser.Visitors;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Misc;
using OracleParser.src.Model;
using System.Linq;

namespace OracleParser.src.Visitors
{
    class ProcedureVisitor:PlSqlParserBaseVisitor<Procedure>
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
            var parameter = _parameterVisitor.Visit(context).SetPositionExt(context);
            _Result.AddParametr(parameter);
            return base.VisitParameter(context);
        }

        public override Procedure VisitGeneral_element_part([NotNull] PlSqlParser.General_element_partContext context)
        {
            _Result.AddElement(Helper.ReadElement(context));
            return base.VisitGeneral_element_part(context);
        }
    }
}
