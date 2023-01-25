using System.Collections;

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
		if (!methodSymbol.GetAttributes().Any(attr => attr.AttributeClass.Name == "AnalyzeForThrowAttribute"))
		{
			return;
		}

		// Get the syntax tree of the method body.
		var syntaxTree = methodSymbol.DeclaringSyntaxReferences.First().GetSyntax();
		var methodDeclaration = syntaxTree.DescendantNodes().OfType<MethodDeclarationSyntax>().Single();
		var methodBody = methodDeclaration.Body;

		// Perform symbolic execution on the method body.
		var abstractedSyntaxTree = new AbstractedSyntaxTree(methodBody);
		var walker = new SymbolicExecutionWalker(context.Compilation, context.CancellationToken);
		var analysisResult = walker.Analyze(abstractedSyntaxTree);
		foreach (var exception in analysisResult.UnhandledExceptions)
		{
			// Report a diagnostic if a reachable throw statement was found.
			var diagnostic = Diagnostic.Create(MayThrowDiagnosticDescriptor.DiagnosticDescriptor, exception.Location, exception.Type.Name);
			context.ReportDiagnostic(diagnostic);
		}
	}
}