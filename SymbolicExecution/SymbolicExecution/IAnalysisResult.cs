namespace SymbolicExecution;

public interface IAnalysisResult
{
	ImmutableArray<ISymbolicExecutionException> UnhandledExceptions { get; }
	ImmutableArray<AnalysisFailure> AnalysisFailures { get; }
}