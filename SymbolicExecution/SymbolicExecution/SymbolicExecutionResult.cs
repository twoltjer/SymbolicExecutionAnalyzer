using System.Collections;
using System.Collections.Generic;

namespace SymbolicExecution;

public readonly struct SymbolicExecutionResult : IAnalysisResult
{
	public SymbolicExecutionResult(ImmutableArray<SymbolicExecutionException> unhandledExceptions)
	{
		UnhandledExceptions = unhandledExceptions;
	}

	public ImmutableArray<SymbolicExecutionException> UnhandledExceptions { get; }
}