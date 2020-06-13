using OracleParser.Model;
using System;
using System.Collections.Generic;
using System.Text;
using AntlrOraclePlsql;
using System.Linq;
using Antlr4.Runtime;

namespace OracleParser.Visitors
{
    static class Helper
    {
        public static ParsedLink ReadElement(PlSqlParser.General_element_partContext context)
        {
            PlSqlParser.Id_expressionContext[] Id_expressionContexts = context.id_expression();
            string elementName = string.Join(".", Id_expressionContexts.Select(x => x.GetText()));
            ParsedLink element = new ParsedLink(elementName);
            element.SetPosition(Id_expressionContexts.First());
            element.SetPosition(Id_expressionContexts.Last());
            return element;
        }

        public static ParsedLink ReadElement(PlSqlParser.Type_nameContext context)
        {
            var elementName = context.GetText();
            var element = new ParsedLink(elementName);
            element.SetPosition(context);
            return element;
        }

        public static T SetPositionExt<T>(this T source, ParserRuleContext context) where T : PieceOfCode
        {
            source.SetPosition(context);
            return source;
        }
    }
}
