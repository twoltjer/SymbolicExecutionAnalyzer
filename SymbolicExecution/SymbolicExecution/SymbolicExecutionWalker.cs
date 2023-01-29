namespace SymbolicExecution;

public struct SymbolicExecutionWalker : ISymbolicExecutionWalker
{
	public SymbolicExecutionWalker(Compilation contextCompilation, CancellationToken contextCancellationToken)
	{
		Compilation = contextCompilation;
		CancellationToken = contextCancellationToken;
	}

	private Compilation Compilation { get; }
	private CancellationToken CancellationToken { get; }

	public IAnalysisResult Analyze(ISyntaxNodeAbstraction node)
	{
		return new SymbolicExecutionResult(
			ImmutableArray<ISymbolicExecutionException>.Empty,
			ImmutableArray<AnalysisFailure>.Empty);
	}
}