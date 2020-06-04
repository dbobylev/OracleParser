using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using AntlrOraclePlsql;
using OracleParser.Model;
using OracleParser.Visitors;

namespace OracleParser.Visitors
{
    class PackageBodyVisitor :PlSqlParserBaseVisitor<ParsedPackagePart>
    {
        private MethodVisitor _procedureVisitor = new MethodVisitor();
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
            if (   child is PlSqlParser.Procedure_bodyContext 
                || child is PlSqlParser.Procedure_specContext
                || child is PlSqlParser.Function_bodyContext 
                || child is PlSqlParser.Function_specContext)
            {
                var procedure = _procedureVisitor.Visit(child).SetPositionExt(child);
                _Result.Procedures.Add(procedure);
            }
            else if (child is PlSqlParser.Variable_declarationContext variableContext)
            {
                var name = variableContext.GetChild(0).GetText();
                var pltype = variableContext.GetChild(1).GetText();

                if (name == "PROCEDURE")
                {
                    var procedure = new ParsedMethod(pltype);
                    procedure.SetPosition(variableContext);
                    _Result.Procedures.Add(procedure);
                }
                else
                {
                    var variable = new ParsedVariable(name, pltype);
                    variable.SetPosition(variableContext);

                    var codePosition = new PieceOfCode();
                    codePosition.SetPosition(variableContext.GetChild(0) as ParserRuleContext);
                    variable.NameIdentifierPart = codePosition;

                    _Result.Variables.Add(variable);
                }
            }
            else if (child is PlSqlParser.Type_declarationContext typeContext)
            {
                var NameContext = typeContext.GetChild(1);
                var name = NameContext.GetText();
                var type = new ParsedType(name);
                type.SetPosition(typeContext);
                type.NameIdentifierPart.SetPosition(NameContext as ParserRuleContext);

                _Result.Objects.Add(type);
            }
        }
    }
}
