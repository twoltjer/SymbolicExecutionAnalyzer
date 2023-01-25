namespace SymbolicExecution;

public readonly struct SymbolicExecutionResult : IAnalysisResult
{
	public SymbolicExecutionResult(ImmutableArray<SymbolicExecutionException> unhandledExceptions, ImmutableArray<AnalysisFailure> analysisFailures)
	{
		UnhandledExceptions = unhandledExceptions;
		AnalysisFailures = analysisFailures;
	}

	public ImmutableArray<SymbolicExecutionException> UnhandledExceptions { get; }
	public ImmutableArray<AnalysisFailure> AnalysisFailures { get; }
}