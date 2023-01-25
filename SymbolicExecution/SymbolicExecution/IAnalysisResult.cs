using System.Collections.Generic;

namespace SymbolicExecution;

public interface IAnalysisResult
{
	ImmutableArray<SymbolicExecutionException> UnhandledExceptions { get; }
}