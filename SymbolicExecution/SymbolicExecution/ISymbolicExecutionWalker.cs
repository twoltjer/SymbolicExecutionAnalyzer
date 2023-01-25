namespace SymbolicExecution;

public interface ISymbolicExecutionWalker
{
	IAnalysisResult Analyze(AbstractedSyntaxTree abstractedSyntaxTree);
}