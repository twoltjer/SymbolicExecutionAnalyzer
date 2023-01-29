namespace SymbolicExecution;

public interface ISymbolicExecutionWalker
{
	IAnalysisResult Analyze(ISyntaxNodeAbstraction node);
}
