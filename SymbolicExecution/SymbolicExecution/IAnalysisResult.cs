using System.Collections.Generic;

namespace SymbolicExecution;

public interface IAnalysisResult
{
	IEnumerable<SymbolicExecutionException> UnhandledExceptions { get; }
}