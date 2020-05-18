using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using AntlrOraclePlsql;
using OracleParser.Model;
using OracleParser.src.Model;
using OracleParser.src.Visitors;

namespace OracleParser.Visitors
{
    class PackageBodyVisitor :PlSqlParserBaseVisitor<ParsedPackagePart>
    {
        private FunctionVisitor _functionVisitor = new FunctionVisitor();
        private ProcedureVisitor _procedureVisitor = new ProcedureVisitor();
        private ParsedPackagePart _Result;

        protected override ParsedPackagePart DefaultResult => _Result;

        public PackageBodyVisitor()
        {
            _Result = new ParsedPackagePart();
        }

        public override ParsedPackagePart VisitPackage_obj_body([NotNull] PlSqlParser.Package_obj_bodyContext context)
        {
            ProcessObj(context);
            return base.VisitPackage_obj_body(context);
        }

        public override ParsedPackagePart VisitPackage_obj_spec([NotNull] PlSqlParser.Package_obj_specContext context)
        {
            ProcessObj(context);
            return base.VisitPackage_obj_spec(context);
        }

        public override ParsedPackagePart VisitVariable_declaration([NotNull] PlSqlParser.Variable_declarationContext context)
        {
            var name = context.GetChild(0).GetText();
            var pltype = context.GetChild(1).GetText();

            if (name == "PROCEDURE")
            {
                var procedure = new ParsedProcedure(pltype);
                procedure.SetPosition(context);
                _Result.AddProcedure(procedure);
            }
            else
            {
                var variable = new ParsedVariable(name, pltype);
                variable.SetPosition(context);
                _Result.AddVariable(variable);
            }
            return base.VisitVariable_declaration(context);
        }

        public override ParsedPackagePart VisitCursor_declaration([NotNull] PlSqlParser.Cursor_declarationContext context)
        {
            return base.VisitCursor_declaration(context);
        }

        public override ParsedPackagePart VisitType_declaration([NotNull] PlSqlParser.Type_declarationContext context)
        {
            return base.VisitType_declaration(context);
        }

        private void ProcessObj(ParserRuleContext context)
        {
            ParserRuleContext child = context.GetChild(0) as ParserRuleContext;
            if (child is PlSqlParser.Procedure_bodyContext || child is PlSqlParser.Procedure_specContext)
            {
                var procedure = _procedureVisitor.Visit(child).SetPositionExt(child);
                _Result.AddProcedure(procedure);
            }
            else if (child is PlSqlParser.Function_bodyContext || child is PlSqlParser.Function_specContext)
            {
                var function = _functionVisitor.Visit(child).SetPositionExt(child);
                _Result.AddProcedure(function);
            }
        }
    }
}
