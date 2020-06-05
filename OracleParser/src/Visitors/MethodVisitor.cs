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
    class MethodVisitor:PlSqlParserBaseVisitor<ParsedMethod>
    {
        private ParameterVisitor _parameterVisitor = new ParameterVisitor();
        protected ParsedMethod _Result;

        protected override ParsedMethod DefaultResult => _Result;

        public override ParsedMethod Visit(IParseTree tree)
        {
            // Что бы узнать процедура или функция
            // var methodTypeName = (tree.GetChild(0) as TerminalNodeImpl).Symbol.Text;

            var identifierContext = tree.GetChild(1);
            if (!(identifierContext is PlSqlParser.IdentifierContext))
                throw new NotImplementedException("Ожидалось имя процедуры");
            _Result = new ParsedMethod(identifierContext.GetText());
            
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

        public override ParsedMethod VisitParameter([NotNull] PlSqlParser.ParameterContext context)
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

        public override ParsedMethod VisitGeneral_element_part([NotNull] PlSqlParser.General_element_partContext context)
        {
            _Result.AddElement(Helper.ReadElement(context));
            return base.VisitGeneral_element_part(context);
        }

        public override ParsedMethod VisitType_spec([NotNull] PlSqlParser.Type_specContext context)
        {
            _Result.ReturnType = context.GetText();
            return base.VisitType_spec(context);
        }
    }
}
