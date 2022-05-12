namespace SymbolicExecution.Analysis;

public interface IHandler<in T>
{
	bool CanHandle(T node);
	SymbolicAnalysisContext Handle(T node, SymbolicAnalysisContext analysisContext, CodeBlockAnalysisContext codeBlockContext);
}