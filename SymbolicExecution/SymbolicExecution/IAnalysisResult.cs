namespace SymbolicExecution;

/// <summary>
/// The result of a symbolic execution analysis, including any unhandled exceptions and analysis failures. Overflows
/// are included in the unhandled exceptions.
/// </summary>
public interface IAnalysisResult
{
	ImmutableArray<ISymbolicExecutionException> UnhandledExceptions { get; }
	ImmutableArray<AnalysisFailure> AnalysisFailures { get; }
}