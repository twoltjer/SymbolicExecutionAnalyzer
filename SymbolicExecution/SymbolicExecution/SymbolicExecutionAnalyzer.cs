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

		context.RegisterSemanticModelAction(AnalyzeSymbol);
	}

	private static void AnalyzeSymbol(SemanticModelAnalysisContext context)
	{
		// Check if the method has the attribute that indicates it should be analyzed
		// for reachable throw statements.
		var model = context.SemanticModel;
		var abstractedSyntaxTree = new AbstractedSyntaxTree(model);
		var abstraction = abstractedSyntaxTree.GetRoot();
		var methodsToAnalyze = abstraction.GetDescendantNodes(includeSelf: true)
			.OfType<IMethodDeclarationSyntaxAbstraction>()
			.Where(syntaxAbstraction => syntaxAbstraction.Symbol is IMethodSymbol methodSymbol && methodSymbol.GetAttributes().Any(IsSymbolicallyAnalyzeAttribute))
			.ToArray();

		foreach (var method in methodsToAnalyze)
		{
			var methodAnalyzer = new AbstractMethodAnalyzer();
			var methodAnalysis = methodAnalyzer.Analyze(method);
			foreach (var failure in methodAnalysis.AnalysisFailures)
			{
				var diagnostic = Diagnostic.Create(AnalysisFailureDiagnosticDescriptor.DiagnosticDescriptor, failure.Location, failure.Reason);
			}
			foreach (var exception in methodAnalysis.UnhandledExceptions)
			{
				// Report a diagnostic if a reachable throw statement was found.
				var diagnostic = Diagnostic.Create(MayThrowDiagnosticDescriptor.DiagnosticDescriptor, exception.Location, exception.Type.Name);
				context.ReportDiagnostic(diagnostic);
			}
		}
	}

	private static bool IsSymbolicallyAnalyzeAttribute(AttributeData arg)
	{
		return BuildNamespaceAndName(arg.AttributeClass) == typeof(SymbolicallyAnalyzeAttribute).FullName;
	}

	private static string BuildNamespaceAndName(INamespaceOrTypeSymbol? symbol)
	{
		if (symbol == null)
			return string.Empty;

		var containingNamespaceName = BuildNamespaceAndName(symbol.ContainingNamespace);
		return string.IsNullOrEmpty(containingNamespaceName)
			? symbol.Name
			: $"{containingNamespaceName}.{symbol.Name}";
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

public interface IAbstractMethodAnalyzer
{
}