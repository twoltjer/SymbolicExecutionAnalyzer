namespace SymbolicExecution;

public class AbstractMethodAnalyzer : IAbstractMethodAnalyzer
{
	public IAnalysisResult Analyze(IMethodDeclarationSyntaxAbstraction method)
	{
		if (!method.Children.OfType<IBlockSyntaxAbstraction>().TryGetSingle(out var blockSyntaxAbstraction))
		{
			return new SymbolicExecutionResult(
				ImmutableArray<SymbolicExecutionException>.Empty,
				new[] { new AnalysisFailure($"Could not get an {nameof(IBlockSyntaxAbstraction)} from the method {method}", method.SourceLocation) }.ToImmutableArray()
				);
		}

		return new SymbolicExecutionResult(
			ImmutableArray<SymbolicExecutionException>.Empty,
			ImmutableArray<AnalysisFailure>.Empty
			);
	}
}