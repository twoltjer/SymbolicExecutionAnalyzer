namespace SymbolicExecution;

public class AbstractMethodAnalyzer : IAbstractMethodAnalyzer
{
	public SymbolicExecutionResult Analyze(IMethodDeclarationSyntaxAbstraction method)
	{
		if (!method.Children.OfType<IBlockSyntaxAbstraction>().TryGetSingle(out var blockSyntaxAbstraction))
		{
			return new SymbolicExecutionResult(
				ImmutableArray<ISymbolicExecutionException>.Empty,
				new[] { new AnalysisFailure($"Could not get an {nameof(IBlockSyntaxAbstraction)} from the method {method}", method.Location) }.ToImmutableArray()
				);
		}

		var symbolicExecutionState = new SymbolicExecutionState();

		var resultStates = blockSyntaxAbstraction.AnalyzeNode(symbolicExecutionState);

		if (!resultStates.IsT1)
			return new SymbolicExecutionResult(
				ImmutableArray<ISymbolicExecutionException>.Empty,
				new[] { resultStates.T2Value }.ToImmutableArray()
				);

		var unhandledExceptions = resultStates.T1Value
			.Select(state => state.CurrentException)
			.Where(exception => exception.HasValue)
			.Select(exception => exception!.Value)
			.Distinct()
			.Select(ConvertToResultException)
			.ToImmutableArray();
		return new SymbolicExecutionResult(
			unhandledExceptions,
			ImmutableArray<AnalysisFailure>.Empty
			);
	}

	private ISymbolicExecutionException ConvertToResultException(ExceptionThrownState exceptionState)
	{
		return new SymbolicExecutionException(exceptionState.Location, exceptionState.Exception.ActualTypeSymbol);
	}
}