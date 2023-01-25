using System.Threading;

namespace SymbolicExecution;

public struct SymbolicExecutionWalker : ISymbolicExecutionWalker<SymbolicExecutionResult>
{
	public SymbolicExecutionWalker(Compilation contextCompilation, CancellationToken contextCancellationToken)
	{
		Compilation = contextCompilation;
		CancellationToken = contextCancellationToken;
	}

	private Compilation Compilation { get; }
	private CancellationToken CancellationToken { get; }

	public SymbolicExecutionResult Analyze(ISyntaxNodeAbstraction node)
	{
		return new SymbolicExecutionResult(ImmutableArray<SymbolicExecutionException>.Empty, ImmutableArray<AnalysisFailure>.Empty);
	}
}