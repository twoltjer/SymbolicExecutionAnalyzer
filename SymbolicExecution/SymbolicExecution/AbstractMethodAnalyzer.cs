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

		var methodSymbol = method.Symbol as IMethodSymbol;
		if (methodSymbol is null)
		{
			return new SymbolicExecutionResult(
				ImmutableArray<ISymbolicExecutionException>.Empty,
				new[] { new AnalysisFailure($"Could not get an {nameof(IMethodSymbol)} from the method {method}", method.Location) }.ToImmutableArray()
				);
		}

		var symbolicExecutionState = SymbolicExecutionState.CreateInitialState(methodSymbol);
		
		var resultStates = blockSyntaxAbstraction.AnalyzeNode(symbolicExecutionState);

		if (!resultStates.IsT1)
			return new SymbolicExecutionResult(
				ImmutableArray<ISymbolicExecutionException>.Empty,
				new[] { resultStates.T2Value }.ToImmutableArray()
				);

		var exceptionThrownStates = resultStates.T1Value
			.Select(state => state.CurrentException)
			.Where(exception => exception != null)
			.Select(exception => exception!)
			.ToList();
		var unhandledExceptions = exceptionThrownStates
			.Distinct()
			.SelectMany(ConvertToResultException)
			.Distinct()
			.ToImmutableArray();
		return new SymbolicExecutionResult(
			unhandledExceptions,
			ImmutableArray<AnalysisFailure>.Empty
			);
	}

	private IEnumerable<ISymbolicExecutionException> ConvertToResultException(IExceptionThrownState exceptionState)
	{
		yield return new SymbolicExecutionException(exceptionState.Location, exceptionState.Exception.ActualTypeSymbol, exceptionState.MethodSymbol);
		
		foreach (var invocationLocation in exceptionState.InvocationLocations)
			yield return new SymbolicExecutionException(invocationLocation.location, exceptionState.Exception.ActualTypeSymbol, invocationLocation.methodSymbol);
	}
}