namespace SymbolicExecution.Architecture;

public interface IHandler<in T>
{
	bool CanHandle(T node);
	SymbolicAnalysisContext Handle(T node, SymbolicAnalysisContext analysisContext, CodeBlockAnalysisContext codeBlockContext);
}