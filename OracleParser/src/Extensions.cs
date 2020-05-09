using Antlr4.Runtime;
using OracleParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser
{
    public static class Extensions
    {
        public static T SetPositionExt<T>(this T source, ParserRuleContext context) where T: PieceOfCode
        {
            source.SetPosition(context);
            return source;
        }
    }
}
