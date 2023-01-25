using System.Collections;
using System.Collections.Generic;

namespace SymbolicExecution;

public readonly struct SymbolicExecutionResult : IAnalysisResult
{
	public IEnumerable<SymbolicExecutionException> UnhandledExceptions { get; }
}