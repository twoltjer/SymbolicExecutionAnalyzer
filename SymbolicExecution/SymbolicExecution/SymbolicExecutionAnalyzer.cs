namespace SymbolicExecution;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[ExcludeFromCodeCoverage]
public class SymbolicExecutionAnalyzer : DiagnosticAnalyzer
{
	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
		MayThrowDiagnosticDescriptor.DiagnosticDescriptor,
		AnalysisFailureDiagnosticDescriptor.DiagnosticDescriptor
		);

	public SyntaxNodeHandler[] SyntaxNodeHandlers => Array.Empty<SyntaxNodeHandler>();
	public override void Initialize(AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();

		context.RegisterCodeBlockAction(AnalyzeSymbol);
	}

	private static void AnalyzeSymbol(CodeBlockAnalysisContext context)
	{
		// Check if code block has existing error diagnostics
		if (context.SemanticModel.GetDiagnostics(context.CodeBlock.Span, context.CancellationToken).Any(d => d.Severity == DiagnosticSeverity.Error))
		{
			return;
		}
		// Check if the method has the attribute that indicates it should be analyzed
		// for reachable throw statements.
		var model = context.SemanticModel;
		var abstractedSyntaxTree = new AbstractedSyntaxTree(model);
		var abstractionOrFailure = abstractedSyntaxTree.GetRoot();
		if (!abstractionOrFailure.IsT1)
		{
			context.ReportDiagnostic(abstractionOrFailure.T2Value);
			return;
		}

		var abstraction = abstractionOrFailure.T1Value;
		var methodsToAnalyze = abstraction.GetDescendantNodes(includeSelf: true)
			.OfType<IMethodDeclarationSyntaxAbstraction>()
			.Where(syntaxAbstraction => syntaxAbstraction.Symbol is IMethodSymbol methodSymbol && methodSymbol.GetAttributes().Any(IsSymbolicallyAnalyzeAttribute))
			.ToArray();

		foreach (var method in methodsToAnalyze)
		{
			var methodAnalyzer = new AbstractMethodAnalyzer();
			SymbolicExecutionResult methodAnalysis;
			try
			{
				methodAnalysis = methodAnalyzer.Analyze(method);
			}
			catch (Exception e)
			{
				var failure = new AnalysisFailure(
					$"An exception occurred during analysis: {e}",
					method.Location);
				context.ReportDiagnostic(failure);
				return;
			}
			foreach (var failure in methodAnalysis.AnalysisFailures)
				context.ReportDiagnostic(failure);
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
		return BuildNamespaceAndName(arg.AttributeClass) == "SymbolicExecution.Control.SymbolicallyAnalyzeAttribute";
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
}

public interface IAbstractMethodAnalyzer
{
}