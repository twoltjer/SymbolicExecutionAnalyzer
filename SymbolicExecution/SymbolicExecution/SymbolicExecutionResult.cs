namespace SymbolicExecution;

/// <summary>
/// The result of a symbolic execution analysis, including any unhandled exceptions and analysis failures. Overflows
/// are included in the unhandled exceptions.
/// </summary>
public readonly struct SymbolicExecutionResult : IAnalysisResult
{
	public SymbolicExecutionResult(
		ImmutableArray<ISymbolicExecutionException> unhandledExceptions,
		ImmutableArray<AnalysisFailure> analysisFailures
		)
	{
		UnhandledExceptions = unhandledExceptions;
		AnalysisFailures = analysisFailures;
	}

	public ImmutableArray<ISymbolicExecutionException> UnhandledExceptions { get; }
	public ImmutableArray<AnalysisFailure> AnalysisFailures { get; }
}