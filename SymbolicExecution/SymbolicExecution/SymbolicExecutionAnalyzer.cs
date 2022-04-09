using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SymbolicExecution
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class SymbolicExecutionAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "SymbolicExecution";
		private const string Category = "Naming";

		// You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
		// See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
		private static readonly LocalizableString Title = new LocalizableResourceString(
			nameof(SymbolicExecutionStrings.AnalyzerTitle),
			SymbolicExecutionStrings.ResourceManager,
			typeof(SymbolicExecutionStrings)
			);

		private static readonly LocalizableString MessageFormat = new LocalizableResourceString(
			nameof(SymbolicExecutionStrings.AnalyzerMessageFormat),
			SymbolicExecutionStrings.ResourceManager,
			typeof(SymbolicExecutionStrings)
			);

		private static readonly LocalizableString Description = new LocalizableResourceString(
			nameof(SymbolicExecutionStrings.AnalyzerDescription),
			SymbolicExecutionStrings.ResourceManager,
			typeof(SymbolicExecutionStrings)
			);

		private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
			DiagnosticId,
			Title,
			MessageFormat,
			Category,
			DiagnosticSeverity.Warning,
			true,
			Description
			);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

		public override void Initialize(AnalysisContext context)
		{
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
			context.EnableConcurrentExecution();

			// TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
			// See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
			context.RegisterCompilationStartAction(RegisterCompilationStart);
			//context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
		}

		private void RegisterCompilationStart(CompilationStartAnalysisContext startContext)
		{
			using (var logger = new FileLogger(startContext.Compilation.AssemblyName))
			{
				var optionsProvider = startContext.Options.AnalyzerConfigOptionsProvider;
				startContext.RegisterCodeBlockAction(
					actionContext =>
						AnalyzeCodeBlock(actionContext, optionsProvider)
					);
			}
		}

		private void AnalyzeCodeBlock(
			CodeBlockAnalysisContext context,
			AnalyzerConfigOptionsProvider optionsProvider
			)
		{
			if (context.OwningSymbol is IMethodSymbol methodSymbol && methodSymbol.GetAttributes()
					.Any(x => x.AttributeClass.Name == "SymbolicallyAnalyzeAttribute"))
				AnalyzeCodeBlock(context);
		}

		private void AnalyzeNode(SyntaxNode node, SymbolicAnalysisContext context)
		{
			switch (node)
			{
				case StatementSyntax statement:
					AnalyzeStatement(statement, context);
					break;
				// Ignore all other nodes
				case AttributeListSyntax _:
				case PredefinedTypeSyntax _:
				case ParameterListSyntax _:
					break;
				default:
					Debug.Fail($"Unexpected node type: {node.GetType()}");
					break;
			}
		}

		private void AnalyzeStatement(StatementSyntax statement, SymbolicAnalysisContext context)
		{
			switch (statement)
			{
				case BlockSyntax block:
					foreach (var child in block.ChildNodes())
						AnalyzeNode(child, context);
					break;
				case ThrowStatementSyntax throwStatement:
					throw new ExceptionStatementException(throwStatement);
					break;
				case IfStatementSyntax ifStatement:
					AnalyzeIfStatement(ifStatement, context);
					break;
				default:
					Debug.Fail("Unhandled node type: " + statement.GetType().Name);
					break;
			}
		}

		private void AnalyzeIfStatement(IfStatementSyntax ifStatement, SymbolicAnalysisContext symbolicAnalysisContext)
		{
			var condition = ifStatement.Condition;
			var trueContext = symbolicAnalysisContext.WithCondition(condition);
			if (trueContext.CanBeTrue())
				AnalyzeStatement(ifStatement.Statement, trueContext);
			if (ifStatement.Else != null)
			{
				var falseContext = symbolicAnalysisContext.WithCondition(NegateCondition(condition));
				if (falseContext.CanBeTrue())
					AnalyzeStatement(ifStatement.Else.Statement, falseContext);
			}
		}

		private ExpressionSyntax NegateCondition(ExpressionSyntax condition)
		{
			Debug.Fail("Not implemented");
			throw new NotImplementedException();
		}

		private void AnalyzeCodeBlock(CodeBlockAnalysisContext context)
		{
			try
			{
				foreach (var node in context.CodeBlock.ChildNodes()) AnalyzeNode(node, SymbolicAnalysisContext.Empty);
			}
			catch (ExceptionStatementException ex)
			{
				var statement = ex.ThrowStatement;
				var diagnostic = Diagnostic.Create(Rule, statement.GetLocation());
				context.ReportDiagnostic(diagnostic);
			}
		}
	}

	internal class SymbolicAnalysisContext
	{
		private SymbolicAnalysisContext(ImmutableArray<ExpressionSyntax> conditions)
		{
			Conditions = conditions;
		}

		public SymbolicAnalysisContext WithCondition(ExpressionSyntax condition)
		{
			return new SymbolicAnalysisContext(Conditions.Append(condition).ToImmutableArray());
		}

		private ImmutableArray<ExpressionSyntax> Conditions { get; }
		public static SymbolicAnalysisContext Empty { get; } = new SymbolicAnalysisContext(ImmutableArray<ExpressionSyntax>.Empty);

		public bool CanBeTrue()
		{
			foreach (var condition in Conditions)
			{
				switch (condition)
				{
					case LiteralExpressionSyntax literal:
						switch (literal.Token.ValueText)
						{
							case "true":
								return true;
							case "false":
								return false;
							default:
								Debug.Fail("Unexpected literal value: " + literal.Token.ValueText);
								return true;
						}
					default:
						Debug.Fail("Unhandled condition type");
						break;
				}
			}

			return true;
		}
	}

	internal class ExceptionStatementException : Exception
	{
		public ExceptionStatementException(ThrowStatementSyntax throwStatement)
		{
			ThrowStatement = throwStatement;
		}

		public ThrowStatementSyntax ThrowStatement { get; }
	}

	public class FileLogger : IDisposable
	{
		private readonly StreamWriter _writer;

		public FileLogger(string filePath)
		{
			_writer = new StreamWriter(filePath, true);
		}

		public void Dispose()
		{
			_writer.Dispose();
		}

		[SuppressMessage(category: "ReSharper", checkId: "UnusedMember.Global")]
		public void WriteLine(string message)
		{
			_writer.WriteLine(message);
			_writer.Flush();
		}
	}
}