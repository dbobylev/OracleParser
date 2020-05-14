using OracleParser.src.Model;
using System;
using System.Collections.Generic;
using System.Text;
using AntlrOraclePlsql;
using System.Linq;
using Antlr4.Runtime;
using OracleParser.Model;

namespace OracleParser.src.Visitors
{
    static class Helper
    {
        public static Element ReadElement(PlSqlParser.General_element_partContext context)
        {
            PlSqlParser.Id_expressionContext[] Id_expressionContexts = context.id_expression();
            string elementName = string.Join(".", Id_expressionContexts.Select(x => x.GetText()));
            Element element = new Element(elementName);
            element.SetPosition(Id_expressionContexts.First());
            element.SetPosition(Id_expressionContexts.Last());
            return element;
        }

        public static T SetPositionExt<T>(this T source, ParserRuleContext context) where T : PieceOfCode
        {
            source.SetPosition(context);
            return source;
        }
    }
}
