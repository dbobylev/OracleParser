using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text;
using Antlr4.Runtime.Tree;
using AntlrOraclePlsql;
using OracleParser.Model;
using OracleParser.src.Visitors;

namespace OracleParser.Visitors
{
    class PackageBodyVisitor :PlSqlParserBaseVisitor<PackageBody>
    {
        private FunctionVisitor _functionVisitor = new FunctionVisitor();
        private ProcedureVisitor _procedureVisitor = new ProcedureVisitor();
        private PackageBody _Result;

        protected override PackageBody DefaultResult => _Result;

        public PackageBodyVisitor()
        {
            _Result = new PackageBody();
        }

        public override PackageBody VisitPackage_obj_body([NotNull] PlSqlParser.Package_obj_bodyContext context)
        {
            Seri.Log.Verbose($"Visit: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            IParseTree child = context.GetChild(0);
            if (child is PlSqlParser.Procedure_bodyContext procedureContext)
            {
                Seri.Log.Verbose("Find Procedure");
                var procedure = _procedureVisitor.Visit(procedureContext).SetPositionExt(procedureContext);
                _Result.AddProcedure(procedure);
            }
            else if (child is PlSqlParser.Function_bodyContext functionContext)
            {
                Seri.Log.Verbose("Find Function");
                var function = _functionVisitor.Visit(functionContext).SetPositionExt(functionContext);
                _Result.AddProcedure(function);
            }

            return base.VisitPackage_obj_body(context);
        }
    }
}
