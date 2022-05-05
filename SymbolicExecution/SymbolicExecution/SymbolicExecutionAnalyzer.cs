﻿using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SymbolicExecution.Analysis.Context;
using SymbolicExecution.Analysis.NodeHandling;

namespace SymbolicExecution
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class SymbolicExecutionAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "SE0001";
		private const string Category = "Naming";

		// You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
		// See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
		private static readonly LocalizableString _title = new LocalizableResourceString(
			nameof(SymbolicExecutionStrings.AnalyzerTitle),
			SymbolicExecutionStrings.ResourceManager,
			typeof(SymbolicExecutionStrings)
			);

		private static readonly LocalizableString _messageFormat = new LocalizableResourceString(
			nameof(SymbolicExecutionStrings.AnalyzerMessageFormat),
			SymbolicExecutionStrings.ResourceManager,
			typeof(SymbolicExecutionStrings)
			);

		private static readonly LocalizableString _description = new LocalizableResourceString(
			nameof(SymbolicExecutionStrings.AnalyzerDescription),
			SymbolicExecutionStrings.ResourceManager,
			typeof(SymbolicExecutionStrings)
			);

		private static readonly DiagnosticDescriptor _rule = new DiagnosticDescriptor(
			DiagnosticId,
			_title,
			_messageFormat,
			Category,
			DiagnosticSeverity.Warning,
			true,
			_description
			);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

		public override void Initialize(AnalysisContext context)
		{
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
			context.EnableConcurrentExecution();

			// TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
			// See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
			// context.RegisterCompilationStartAction(RegisterCompilationStart);
			context.RegisterCodeBlockAction(RegisterAnalyzeCodeBlock);
			//context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
		}

		private void RegisterAnalyzeCodeBlock(
			CodeBlockAnalysisContext codeBlockContext
			)
		{
			if (codeBlockContext.OwningSymbol is IMethodSymbol methodSymbol && methodSymbol.GetAttributes()
					.Any(x => x.AttributeClass.Name == "SymbolicallyAnalyzeAttribute"))
				AnalyzeCodeBlock(codeBlockContext);
		}

		private void AnalyzeCodeBlock(CodeBlockAnalysisContext codeBlockContext)
		{
			if (codeBlockContext.CodeBlock is not MethodDeclarationSyntax methodDeclaration)
			{
				throw new UnhandledSyntaxException();
			}

			if (methodDeclaration.GetDiagnostics().Any(x => x.Severity == DiagnosticSeverity.Error))
				return;
			try
			{
				var analysisContext = SymbolicAnalysisContext.Empty;
				foreach (var node in codeBlockContext.CodeBlock.ChildNodes())
				{
					analysisContext = NodeHandlerMediator.Instance.Handle(node, analysisContext, codeBlockContext);
				}
			}
			catch (ExceptionStatementException ex)
			{
				var statement = ex.ThrowStatement;
				var diagnostic = Diagnostic.Create(_rule, statement.GetLocation(), statement.ToString());
				codeBlockContext.ReportDiagnostic(diagnostic);
			}
		}
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

	public interface IValueScope : IEquatable<IValueScope>
	{
		IValueScope? Union(IValueScope other);
		IValueScope? Intersection(IValueScope other);
	}

	public sealed class UninitializedValueScope : IValueScope
	{
		public static UninitializedValueScope Instance { get; } = new UninitializedValueScope();

		public IValueScope Union(IValueScope other) =>
			throw new ValidationFailedException("UninitializedValueScope cannot be unioned");

		public IValueScope Intersection(IValueScope other) =>
			throw new ValidationFailedException("UninitializedValueScope cannot be intersected");

		public bool Equals(IValueScope other) => other is UninitializedValueScope;
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
		public IValueScope? Union(IValueScope other)
		{
			if (other is ConcreteValueScope<T> otherConcreteValueScope)
			{
				if (Value.Equals(otherConcreteValueScope.Value))
					return this;

				return EmptyValueScope.Instance;
			}

			throw new UnexpectedValueException();
		}

		public IValueScope? Intersection(IValueScope other)
		{
			if (other is ConcreteValueScope<T> otherConcreteValueScope)
			{
				if (Value.Equals(otherConcreteValueScope.Value))
					return this;

				return EmptyValueScope.Instance;
			}

			throw new UnexpectedValueException();
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

	internal class UnhandledSyntaxException : Exception
	{
	}

	internal class UnexpectedValueException : Exception
	{
	}

	internal class ValidationFailedException : Exception
	{
		public ValidationFailedException(string message) : base(message)
		{
		}
	}
}