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
		
		if (!method.Children.OfType<IParameterListSyntaxAbstraction>().TryGetSingle(out var parameterListSyntaxAbstraction))
		{
			return new SymbolicExecutionResult(
				ImmutableArray<ISymbolicExecutionException>.Empty,
				new[] { new AnalysisFailure($"Could not get an {nameof(IParameterListSyntaxAbstraction)} from the method {method}", method.Location) }.ToImmutableArray()
				);
		}

		var symbolicExecutionState = SymbolicExecutionState.CreateInitialState();

		var afterParameterAnalyses = parameterListSyntaxAbstraction.AnalyzeNode(symbolicExecutionState);
		if (!afterParameterAnalyses.IsT1)
			return new SymbolicExecutionResult(
				ImmutableArray<ISymbolicExecutionException>.Empty,
				new[] { afterParameterAnalyses.T2Value }.ToImmutableArray()
				);
		var resultStates = new List<IAnalysisState>();
		foreach (var state in afterParameterAnalyses.T1Value)
		{
			var results = blockSyntaxAbstraction.AnalyzeNode(state);
			if (results.IsT1)
				resultStates.AddRange(results.T1Value);
			else
				return new SymbolicExecutionResult(
					ImmutableArray<ISymbolicExecutionException>.Empty,
					new[] { results.T2Value }.ToImmutableArray()
					);
		}

		var unhandledExceptions = resultStates
			.Select(state => state.CurrentException)
			.Where(exception => exception != null)
			.Select(exception => exception!)
			.Distinct()
			.Select(ConvertToResultException)
			.ToImmutableArray();
		return new SymbolicExecutionResult(
			unhandledExceptions,
			ImmutableArray<AnalysisFailure>.Empty
			);
	}

	private ISymbolicExecutionException ConvertToResultException(IExceptionThrownState exceptionState)
	{
		return new SymbolicExecutionException(exceptionState.Location, exceptionState.Exception.Type);
	}
}