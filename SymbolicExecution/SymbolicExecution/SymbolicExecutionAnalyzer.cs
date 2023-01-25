using System.Collections;
using System.Diagnostics;
using SymbolicExecution.Control;

namespace SymbolicExecution;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class SymbolicExecutionAnalyzer : DiagnosticAnalyzer
{
	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
		MayThrowDiagnosticDescriptor.DiagnosticDescriptor,
		UnexpectedValueDiagnosticDescriptor.DiagnosticDescriptor,
		UnhandledSyntaxDiagnosticDescriptor.DiagnosticDescriptor
		);

	public SyntaxNodeHandler[] SyntaxNodeHandlers => Array.Empty<SyntaxNodeHandler>();
	public override void Initialize(AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();

		context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
	}

	private static void AnalyzeSymbol(SymbolAnalysisContext context)
	{
		// Check if the method has the attribute that indicates it should be analyzed
		// for reachable throw statements.
		var methodSymbol = (IMethodSymbol)context.Symbol;
		if (!methodSymbol.GetAttributes().Any(attr => attr.AttributeClass?.Name == nameof(SymbolicallyAnalyzeAttribute)))
		{
			return;
		}

		// Get the syntax tree of the method body.
		var syntaxTree = methodSymbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();
		var methodDeclaration = syntaxTree as MethodDeclarationSyntax;
		var methodBody = methodDeclaration?.Body;
		if (methodBody == null)
		{
			HandleInvalidContext(context);
			return;
		}

		// Perform symbolic execution on the method body.
		var abstractedSyntaxTree = new AbstractedSyntaxTree(methodBody);
		var walker = new SymbolicExecutionWalker(context.Compilation, context.CancellationToken);
		var analysisResult = walker.Analyze(abstractedSyntaxTree.GetRoot());
		foreach (var exception in analysisResult.UnhandledExceptions)
		{
			// Report a diagnostic if a reachable throw statement was found.
			var diagnostic = Diagnostic.Create(MayThrowDiagnosticDescriptor.DiagnosticDescriptor, exception.Location, exception.Type.Name);
			context.ReportDiagnostic(diagnostic);
		}
	}

	private static void HandleInvalidContext(SymbolAnalysisContext context)
	{
		var location = context.Symbol.Locations.FirstOrDefault();
		if (location == null)
		{
			Debug.Fail("This should be unreachable, as there should be at least one location");
			return;
		}

		var diagnostic = Diagnostic.Create(
			UnexpectedValueDiagnosticDescriptor.DiagnosticDescriptor,
			location
			);
		context.ReportDiagnostic(diagnostic);
	}
}