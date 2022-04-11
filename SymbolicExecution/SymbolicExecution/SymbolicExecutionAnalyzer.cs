using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
			//using (var logger = new FileLogger(startContext.Compilation.AssemblyName))
			//{
				var optionsProvider = startContext.Options.AnalyzerConfigOptionsProvider;
				startContext.RegisterCodeBlockAction(
					actionContext =>
						AnalyzeCodeBlock(actionContext, optionsProvider)
					);
			//}
		}

		private void AnalyzeCodeBlock(
			CodeBlockAnalysisContext codeBlockContext,
			AnalyzerConfigOptionsProvider optionsProvider
			)
		{
			if (codeBlockContext.OwningSymbol is IMethodSymbol methodSymbol && methodSymbol.GetAttributes()
					.Any(x => x.AttributeClass.Name == "SymbolicallyAnalyzeAttribute"))
				AnalyzeCodeBlock(codeBlockContext);
		}

		private void AnalyzeNode(
			SyntaxNode node,
			SymbolicAnalysisContext context,
			CodeBlockAnalysisContext codeBlockAnalysisContext,
			out SymbolicAnalysisContext mutatedContext
			)
		{
			mutatedContext = context;
			switch (node)
			{
				case StatementSyntax statement:
					AnalyzeStatement(statement, context, codeBlockAnalysisContext, out mutatedContext);
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

		private void AnalyzeStatement(
			StatementSyntax statement,
			SymbolicAnalysisContext context,
			CodeBlockAnalysisContext codeBlockAnalysisContext,
			out SymbolicAnalysisContext mutatedContext
			)
		{
			mutatedContext = context;
			switch (statement)
			{
				case BlockSyntax block:
					foreach (var child in block.ChildNodes())
						AnalyzeNode(child, mutatedContext, codeBlockAnalysisContext, out mutatedContext);
					break;
				case ThrowStatementSyntax throwStatement:
					throw new ExceptionStatementException(throwStatement);
				case IfStatementSyntax ifStatement:
					AnalyzeIfStatement(ifStatement, context, codeBlockAnalysisContext);
					break;
				case LocalDeclarationStatementSyntax localDeclarationStatement:
					AnalyzeLocalDeclarationStatement(localDeclarationStatement, context, codeBlockAnalysisContext, out mutatedContext);
					break;
				default:
					Debug.Fail("Unhandled node type: " + statement.GetType().Name);
					break;
			}
		}

		private void AnalyzeLocalDeclarationStatement(
			LocalDeclarationStatementSyntax localDeclarationStatement,
			SymbolicAnalysisContext context,
			CodeBlockAnalysisContext codeBlockAnalysisContext,
			out SymbolicAnalysisContext mutatedContext
			)
		{
			mutatedContext = context;
			foreach (var variable in localDeclarationStatement.Declaration.Variables)
			{
				var variableName = variable.Identifier.Text;
				var variableSymbol = ModelExtensions.GetDeclaredSymbol(codeBlockAnalysisContext.SemanticModel, variable) as ILocalSymbol;
				var type = variableSymbol?.Type;
				var specialType = type?.SpecialType;
				var initializer = variable.Initializer;
				var initializerValue = initializer?.Value;
				var initializedValue = initializerValue?.ToString();
				switch (specialType)
				{
					case SpecialType.System_Int32:
						var valueScope = initializedValue != null
							? new ConcreteValueScope<int>(int.Parse(initializedValue))
							: (IValueScope) UninitializedValueScope.Instance;
						mutatedContext = mutatedContext.WithDeclaration(new VariableInfo(variableName, specialType.Value, valueScope));
						break;
					default:
						Debug.Fail($"Unexpected type: {type}");
						break;
				}
				
			}
		}

		private void AnalyzeIfStatement(IfStatementSyntax ifStatement, SymbolicAnalysisContext symbolicAnalysisContext, CodeBlockAnalysisContext codeBlockAnalysisContext)
		{
			var condition = ifStatement.Condition;
			var trueContext = symbolicAnalysisContext.WithCondition(condition);
			if (trueContext.CanBeTrue())
			{
				AnalyzeStatement(ifStatement.Statement, trueContext, codeBlockAnalysisContext, out var mutatedContext);
				Debug.Assert(trueContext == mutatedContext, "IfStatementSyntax should not mutate the context");
			}
			if (ifStatement.Else != null)
			{
				var falseContext = symbolicAnalysisContext.WithCondition(NegateCondition(condition));
				if (falseContext.CanBeTrue())
				{
					AnalyzeStatement(ifStatement.Else.Statement, falseContext, codeBlockAnalysisContext, out var mutatedContext);
					Debug.Assert(falseContext == mutatedContext, "IfStatementSyntax should not mutate the context");
				}
			}
		}

		private ExpressionSyntax NegateCondition(ExpressionSyntax condition)
		{
			Debug.Fail("Not implemented");
			throw new NotImplementedException();
		}

		private void AnalyzeCodeBlock(CodeBlockAnalysisContext codeBlockContext)
		{
			try
			{
				var analysisContext = SymbolicAnalysisContext.Empty;
				foreach (var node in codeBlockContext.CodeBlock.ChildNodes())
				{
					// Lines in a code block can mutate context; it will be changing as the statements are analyzed
					AnalyzeNode(node, analysisContext, codeBlockContext, out analysisContext);
				};
			}
			catch (ExceptionStatementException ex)
			{
				var statement = ex.ThrowStatement;
				var diagnostic = Diagnostic.Create(Rule, statement.GetLocation());
				codeBlockContext.ReportDiagnostic(diagnostic);
			}
		}
	}

	internal class SymbolicAnalysisContext
	{
		private SymbolicAnalysisContext(
			ImmutableArray<ExpressionSyntax> conditions,
			ImmutableArray<VariableInfo> variables
			)
		{
			Conditions = conditions;
			Variables = variables;
		}

		public SymbolicAnalysisContext WithCondition(ExpressionSyntax condition)
		{
			return new SymbolicAnalysisContext(Conditions.Append(condition).ToImmutableArray(), Variables);
		}

		private ImmutableArray<ExpressionSyntax> Conditions { get; }
		public static SymbolicAnalysisContext Empty { get; } = new SymbolicAnalysisContext(ImmutableArray<ExpressionSyntax>.Empty, ImmutableArray<VariableInfo>.Empty);

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
					case BinaryExpressionSyntax binaryExpression:
						return BinaryExpressionCanBeTrue(binaryExpression);
					default:
						Debug.Fail("Unhandled condition type");
						break;
				}
			}

			return true;
		}

		private bool BinaryExpressionCanBeTrue(BinaryExpressionSyntax binaryExpression)
		{
			var left = binaryExpression.Left;
			var right = binaryExpression.Right;
			switch (binaryExpression.Kind())
			{
				case SyntaxKind.EqualsExpression:
					return CanBeEqual(left, right);
				case SyntaxKind.NotEqualsExpression:
					return CanBeNotEqual(left, right);
				case SyntaxKind.GreaterThanExpression:
				case SyntaxKind.GreaterThanOrEqualExpression:
				case SyntaxKind.LessThanExpression:
				case SyntaxKind.LessThanOrEqualExpression:
				default:
					Debug.Fail("Unexpected binary expression: " + binaryExpression.Kind());
					return true;
			}
		}

		private bool CanBeNotEqual(ExpressionSyntax left, ExpressionSyntax right)
		{
			var leftValueScope = GetValueScope(left);
			var rightValueScope = GetValueScope(right);
			if (leftValueScope is null || rightValueScope is null)
			{
				Debug.Fail("Unexpected null value scope");
				return true;
			}

			var union = leftValueScope.Union(rightValueScope);
			
			return !(union.Equals(leftValueScope) && union.Equals(rightValueScope));
		}


		private IValueScope GetValueScope(ExpressionSyntax expressionSyntax)
		{
			switch (expressionSyntax)
			{
				case IdentifierNameSyntax identifierName:
					return GetVariableValueScope(identifierName.Identifier.ValueText);
				case LiteralExpressionSyntax literal:
					if (int.TryParse(literal.Token.ValueText, out var intValue))
					{
						return new ConcreteValueScope<int>(intValue);
					}
					else
					{
						Debug.Fail("Unexpected literal value: " + literal.Token.ValueText);
						return new ConcreteValueScope<int>(0);
					}
				default:
					Debug.Fail("Unexpected expression type: " + expressionSyntax.Kind());
					return null;
			}
		}

		private IValueScope GetLiteralValueScope(string valueText)
		{
			throw new NotImplementedException();
		}

		private IValueScope GetVariableValueScope(string identifierValueText)
		{
			var matchingVariables = Variables.Where(v => v.Name == identifierValueText).ToList();
			if (matchingVariables.Count == 1)
			{
				return matchingVariables.First().ValueScope;
			}
			else
			{
				return null;
			}
		}

		private bool CanBeEqual(ExpressionSyntax left, ExpressionSyntax right)
		{
			var leftValueScope = GetValueScope(left);
			var rightValueScope = GetValueScope(right);
			if (leftValueScope is null || rightValueScope is null)
			{
				Debug.Fail("Unexpected null value scope");
				return true;
			}

			var intersection = leftValueScope.Intersection(rightValueScope);

			return !(intersection.Equals(EmptyValueScope.Instance));
		}

		public SymbolicAnalysisContext WithDeclaration(VariableInfo variable)
		{
			return new SymbolicAnalysisContext(Conditions, Variables.Append(variable).ToImmutableArray());
		}

		public ImmutableArray<VariableInfo> Variables { get; }
	}

	internal sealed class EmptyValueScope : IValueScope
	{
		internal static readonly EmptyValueScope Instance = new EmptyValueScope();
		
		private EmptyValueScope()
		{
		}

		public IValueScope Union(IValueScope other) => this;
		
		public IValueScope Intersection(IValueScope other) => other;
		
		public bool Equals(IValueScope other) => other is EmptyValueScope;
	}

	internal struct VariableInfo
	{
		public VariableInfo(string name, SpecialType type, IValueScope valueScope)
		{
			Name = name;
			Type = type;
			ValueScope = valueScope;
		}

		public string Name { get; }
		public SpecialType Type { get; }
		public IValueScope ValueScope { get; }
	}

	public interface IValueScope : IEquatable<IValueScope>
	{
		IValueScope Union(IValueScope other);
		IValueScope Intersection(IValueScope other);
	}
	
	public sealed class UninitializedValueScope : IValueScope
	{
		public static UninitializedValueScope Instance { get; } = new UninitializedValueScope();
		public IValueScope Union(IValueScope other)
		{
			Debug.Fail("Unexpected call to Union on UninitializedValueScope");
			return this;
		}
		
		public IValueScope Intersection(IValueScope other)
		{
			Debug.Fail("Unexpected call to Intersection on UninitializedValueScope");
			return this;
		}
		
		public bool Equals(IValueScope other)
		{
			return other is UninitializedValueScope;
		}
	}
	
	public interface IConcreteValueScope : IValueScope
	{
	}
	
	public sealed class ConcreteValueScope<T> : IConcreteValueScope where T : unmanaged
	{
		public ConcreteValueScope(T value)
		{
			Value = value;
		}

		public T Value { get; }
		public IValueScope Union(IValueScope other)
		{
			if (other is ConcreteValueScope<T> otherConcreteValueScope)
			{
				if (Value.Equals(otherConcreteValueScope.Value))
					return this;

				return EmptyValueScope.Instance;
			}

			Debug.Fail("Unexpected value scope type");
			return null;
		}
		
		public IValueScope Intersection(IValueScope other)
		{
			if (other is ConcreteValueScope<T> otherConcreteValueScope)
			{
				if (Value.Equals(otherConcreteValueScope.Value))
					return this;

				return EmptyValueScope.Instance;
			}

			Debug.Fail("Unexpected value scope type");
			return null;
		}

		public bool Equals(IValueScope other)
		{
			return other is ConcreteValueScope<T> otherConcreteValueScope && Value.Equals(otherConcreteValueScope.Value);
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