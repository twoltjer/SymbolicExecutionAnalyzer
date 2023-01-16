using System.Threading;

namespace SymbolicExecution;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class SymbolicExecutionAnalyzer : DiagnosticAnalyzer
{
	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
		MayThrowDiagnosticDescriptor.DiagnosticDescriptor,
		UnexpectedValueDiagnosticDescriptor.DiagnosticDescriptor,
		UnhandledSyntaxDiagnosticDescriptor.DiagnosticDescriptor
		);
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
		if (!methodSymbol.GetAttributes().Any(attr => attr.AttributeClass.Name == "AnalyzeForThrowAttribute"))
		{
			return;
		}

		// Get the syntax tree of the method body.
		var syntaxTree = methodSymbol.DeclaringSyntaxReferences.First().GetSyntax();
		var methodDeclaration = syntaxTree.DescendantNodes().OfType<MethodDeclarationSyntax>().Single();
		var methodBody = methodDeclaration.Body;

		// Perform symbolic execution on the method body.
		var walker = new SymbolicExecutionWalker(context.Compilation, context.CancellationToken);
		var analysisResult = PerformSymbolicExecution(methodBody, context.Compilation, context.ReportDiagnostic, new SyntaxNodeHandler());
		foreach (var exception in analysisResult.UnhandledExceptions)
		{
			// Report a diagnostic if a reachable throw statement was found.
			var diagnostic = Diagnostic.Create(MayThrowDiagnosticDescriptor.DiagnosticDescriptor, exception.Location, exception.Name);
			context.ReportDiagnostic(diagnostic);
		}
	}

	private static SymbolicExecutionResult PerformSymbolicExecution(SyntaxNode node, Compilation compilation, Action<Diagnostic> reportDiagnostic, SyntaxNodeHandler handler)
	{
		
	}
}

internal struct SymbolicExecutionWalker
{
	public SymbolicExecutionWalker(Compilation contextCompilation, CancellationToken contextCancellationToken)
	{
		Compilation = contextCompilation;
		CancellationToken = contextCancellationToken;
	}

	private Compilation Compilation { get; }
	private CancellationToken CancellationToken { get; }
}