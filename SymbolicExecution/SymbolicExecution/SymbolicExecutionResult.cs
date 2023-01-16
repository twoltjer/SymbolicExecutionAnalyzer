using System.Collections;

namespace SymbolicExecution;

internal readonly struct SymbolicExecutionResult
{
	internal IImmutableList<SymbolicExecutionException> UnhandledExceptions { get; }
}