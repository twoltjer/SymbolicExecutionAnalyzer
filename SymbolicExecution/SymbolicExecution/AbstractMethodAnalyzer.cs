namespace SymbolicExecution;

public class AbstractMethodAnalyzer : IAbstractMethodAnalyzer
{
	public SymbolicExecutionResult Analyze(IMethodDeclarationSyntaxAbstraction method)
	{
		if (!method.Children.OfType<IBlockSyntaxAbstraction>().TryGetSingle(out var blockSyntaxAbstraction))
		{
			return new SymbolicExecutionResult(
				ImmutableArray<SymbolicExecutionException>.Empty,
				new[] { new AnalysisFailure($"Could not get an {nameof(IBlockSyntaxAbstraction)} from the method {method}", method.SourceLocation) }.ToImmutableArray()
				);
		}

		var symbolicExecutionState = new SymbolicExecutionState(ImmutableArray<SymbolicExecutionException>.Empty);

		var resultState = AnalyzeNode(blockSyntaxAbstraction, symbolicExecutionState);

		return new SymbolicExecutionResult(
			resultState.UnhandledExceptions,
			ImmutableArray<AnalysisFailure>.Empty
			);
	}

	private SymbolicExecutionState AnalyzeNode(ISyntaxNodeAbstraction node, SymbolicExecutionState stateBeforeExecution)
	{
		return stateBeforeExecution;
	}
}

public readonly struct SymbolicExecutionState
{
	public SymbolicExecutionState(ImmutableArray<SymbolicExecutionException> unhandledExceptions)
	{
		UnhandledExceptions = unhandledExceptions;
	}

	public ImmutableArray<SymbolicExecutionException> UnhandledExceptions { get; }
}