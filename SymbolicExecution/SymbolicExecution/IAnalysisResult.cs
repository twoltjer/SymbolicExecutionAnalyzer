namespace SymbolicExecution;

public interface IAnalysisResult
{
	ImmutableArray<SymbolicExecutionException> UnhandledExceptions { get; }
	ImmutableArray<AnalysisFailure> AnalysisFailures { get; }
}