using Antlr4.Runtime.Misc;
using AntlrOraclePlsql;
using OracleParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Visitors
{
    class ParameterVisitor :PlSqlParserBaseVisitor<Parameter>
    {
        Parameter _Result;

        protected override Parameter DefaultResult => _Result;

        public override Parameter VisitParameter_name([NotNull] PlSqlParser.Parameter_nameContext context)
        {
            Seri.Log.Verbose($"Visit: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            _Result = new Parameter(context.GetText());
            return base.VisitParameter_name(context);
        }

        public override Parameter VisitType_spec([NotNull] PlSqlParser.Type_specContext context)
        {
            Seri.Log.Verbose($"Visit: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            _Result.SetType(context.GetText());
            return base.VisitType_spec(context);
        }
    }
}
