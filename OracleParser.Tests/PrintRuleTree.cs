using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using AntlrOraclePlsql;
using NUnit.Framework;
using OracleParser.Tests.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleParser.Tests
{
    class PrintRuleTree
    {
		private static int MaxDept = 100;

		[Test]
		public static void PrintRulesofFile()
		{
			ParserRuleContext context =  Analyzer.RunUpperCase(SourceFiles.pathPackageTest);
			PrintChilds(context);
			Assert.Pass();
		}

		[Test]
		public static void PrintSql12c()
		{
			ParserRuleContext context = Analyzer.Run(SourceFiles.sql21cTest);
			PrintChilds(context);
			Assert.Pass();
		}

		[Test]
		public static void PrintInvisColumn12c()
		{
			ParserRuleContext context = Analyzer.Run(SourceFiles.InvisColumn12cTest);
			PrintChilds(context);
			Assert.Pass();
		}

		public static void PrintChilds(IParseTree tree, int indent = 0)
		{
			if (indent > MaxDept) return;

			string pad = new string(Enumerable.Range(0, indent).Select(x => ' ').ToArray());
			string TypeName = tree.GetType().Name;
			string txt = string.Empty;
			if (tree is TerminalNodeImpl treeTNI)
				txt = $" : {treeTNI.Symbol.Text}";
			Console.WriteLine($"{pad}{TypeName}{txt}");

			int c = tree.ChildCount;

			for (int i = 0; i < c; i++)
			{
				PrintChilds(tree.GetChild(i), indent + 2);
			}
		}
	}
}
