using AntlrOraclePlsql;
using System;
using System.Collections.Generic;
using System.Text;
using OracleParser.Model;
using OracleParser.Visitors;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Misc;
using System.Linq;
using OracleParser.Model.PackageModel;
using Antlr4.Runtime;

namespace OracleParser.Visitors
{
    class MethodVisitor:PlSqlParserBaseVisitor<ParsedProcedure>
    {
        private ParameterVisitor _parameterVisitor = new ParameterVisitor();
        protected ParsedProcedure _Result;

        protected override ParsedProcedure DefaultResult => _Result;

        public override ParsedProcedure Visit(IParseTree tree)
        {
            var methodTypeName = (tree.GetChild(0) as TerminalNodeImpl).Symbol.Text;

            var identifierContext = tree.GetChild(1);
            if (!(identifierContext is PlSqlParser.IdentifierContext))
                throw new NotImplementedException("Ожидалось имя процедуры");

            if (methodTypeName == "PROCEDURE")
                _Result = new ParsedProcedure(identifierContext.GetText());
            else
                _Result = new ParsedFunction(identifierContext.GetText());

            var codePosition = new PieceOfCode();
            codePosition.SetPosition(identifierContext as ParserRuleContext);
            _Result.NameIdentifierPart = codePosition;

            int chCnt = tree.ChildCount;
            for (int i = 2; i < chCnt; i++)
                if (tree.GetChild(i) is TerminalNodeImpl terminalNoeImpl)
                    if (terminalNoeImpl.Symbol.Text == "IS" || terminalNoeImpl.Symbol.Text == "AS")
                    {
                        codePosition = new PieceOfCode();
                        codePosition.SetPosition(tree as ParserRuleContext);
                        var PrevChild = tree.GetChild(i - 1);
                        if (PrevChild is ParserRuleContext PrevContext)
                            codePosition.SetPosition(PrevContext);
                        else if (PrevChild is TerminalNodeImpl PrevTerminalNodeImpl)
                            codePosition.SetPosition(PrevTerminalNodeImpl);
                        _Result.DeclarationPart = codePosition;
                        break;
                    }

            return base.Visit(tree);
        }

        public override ParsedProcedure VisitParameter([NotNull] PlSqlParser.ParameterContext context)
        {
            // Условие для того что бы параметры не подхватывались из вложенных методов
            if (context.Parent.Parent is PlSqlParser.Package_obj_bodyContext ||
                context.Parent.Parent is PlSqlParser.Package_obj_specContext)
            {
                var parameter = _parameterVisitor.Visit(context).SetPositionExt(context);
                _Result.AddParametr(parameter);
            }
            return base.VisitParameter(context);
        }

        public override ParsedProcedure VisitGeneral_element_part([NotNull] PlSqlParser.General_element_partContext context)
        {
            _Result.AddElement(Helper.ReadElement(context));
            return base.VisitGeneral_element_part(context);
        }

        public override ParsedProcedure VisitType_name([NotNull] PlSqlParser.Type_nameContext context)
        {
            _Result.AddElement(Helper.ReadElement(context));
            return base.VisitType_name(context);
        }

        public override ParsedProcedure VisitFunction_call([NotNull] PlSqlParser.Function_callContext context)
        {
            _Result.AddElement(Helper.ReadElement(context));
            return base.VisitFunction_call(context);
        }
    }
}
