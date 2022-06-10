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

		// See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
		context.RegisterCodeBlockAction(RegisterAnalyzeCodeBlock);
	}

	private void RegisterAnalyzeCodeBlock(
		CodeBlockAnalysisContext codeBlockAnalysisContext
		)
	{
		if (codeBlockAnalysisContext.OwningSymbol is IMethodSymbol methodSymbol && methodSymbol.GetAttributes()
				.Any(x => x.AttributeClass.Name == "SymbolicallyAnalyzeAttribute"))
			AnalyzeCodeBlock(codeBlockAnalysisContext);
	}

	private async Task<Diagnostic[]> AnalyzeCodeBlockAsync(CodeBlockAnalysisContext codeBlockAnalysisContext, CancellationToken token)
	{
		var convertedSyntax =
			await SyntaxNodeConversionHandlerMediator.Instance.HandleAsync(await codeBlockAnalysisContext.SemanticModel.SyntaxTree.GetRootAsync(token), token);
		if (convertedSyntax.IsFaulted)
		{
			return new[]
			{
				Diagnostic.Create(
					UnhandledSyntaxDiagnosticDescriptor.DiagnosticDescriptor,
					convertedSyntax.ErrorInfo.Location ?? codeBlockAnalysisContext.CodeBlock.GetLocation(),
					convertedSyntax.ErrorInfo.Message
					),
			};
		}

		return Array.Empty<Diagnostic>();
	}

	private void AnalyzeCodeBlock(CodeBlockAnalysisContext codeBlockAnalysisContext)
	{
		if (codeBlockAnalysisContext.CodeBlock is not MethodDeclarationSyntax methodDeclaration)
			// TODO: Produce a diagnostic that the attribute was used incorrectly
			return;

		var methodHasErrors = methodDeclaration.GetDiagnostics().Any(x => x.Severity == DiagnosticSeverity.Error);
		if (methodHasErrors)
			return;

		try
		{
			var runCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
			var alertCts = new CancellationTokenSource(TimeSpan.FromSeconds(40));
			var task = Task.Run(() => AnalyzeCodeBlockAsync(codeBlockAnalysisContext, runCts.Token), runCts.Token);
			while (!task.IsFaulted && !task.IsCompleted && !task.IsCanceled)
			{
				Thread.Sleep(TimeSpan.FromSeconds(0.5));
				if (runCts.IsCancellationRequested)
				{
					Diagnostic.Create(AnalysisTimedOutDiagnosticDescriptor.DiagnosticDescriptor, codeBlockAnalysisContext.CodeBlock.GetLocation());
				}
				if (alertCts.IsCancellationRequested)
				{
					Debug.Fail("Task did not end!");
					break;
				}
			}

			if (task.IsCompleted)
			{
				var diagnostics = task.Result;
				foreach (var diagnostic in diagnostics)
					codeBlockAnalysisContext.ReportDiagnostic(diagnostic);
			}
			// var executionPaths = new List<ExecutionPath> { ExecutionPath.Empty };
			// var faults = new List<AnalysisErrorInfo>();
			// foreach (var node in codeBlockAnalysisContext.CodeBlock.ChildNodes())
			// {
			// 	var nextExecutionPaths = new List<ExecutionPath>();
			// 	foreach (var executionPath in executionPaths)
			// 	{
			// 		var executionPathResults = NodeHandlerMediator.Instance.Handle(
			// 			node,
			// 			executionPath
			// 			);
			// 		foreach (var executionPathResult in executionPathResults)
			// 		{
			// 			if (executionPathResult.IsFaulted)
			// 			{
			// 				var errorInfo = executionPathResult.ErrorInfo;
			// 				if (!faults.Contains(errorInfo))
			// 					faults.Add(errorInfo);
			// 			}
			// 			else
			// 			{
			// 				nextExecutionPaths.Add(executionPathResult.Value);
			// 			}
			// 		}
			// 	}
			// 	executionPaths.Clear();
			// 	executionPaths.AddRange(nextExecutionPaths);
			// 	nextExecutionPaths.Clear();
			// }
		}
		catch (Exception ex)
		{
			Debug.Fail(ex.ToString());
		}
	}
}