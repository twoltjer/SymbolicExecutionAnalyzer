namespace SymbolicExecution;

public interface ISymbolicExecutionWalker<T> where T : IAnalysisResult
{
	T Analyze(ISyntaxNodeAbstraction node);
}