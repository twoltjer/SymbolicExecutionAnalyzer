namespace SymbolicExecution;

public readonly struct SymbolicExecutionResult : IAnalysisResult
{
	public SymbolicExecutionResult(ImmutableArray<ISymbolicExecutionException> unhandledExceptions, ImmutableArray<AnalysisFailure> analysisFailures)
	{
		UnhandledExceptions = unhandledExceptions;
		AnalysisFailures = analysisFailures;
	}

	public ImmutableArray<ISymbolicExecutionException> UnhandledExceptions { get; }
	public ImmutableArray<AnalysisFailure> AnalysisFailures { get; }
}