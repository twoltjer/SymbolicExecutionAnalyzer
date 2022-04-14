using Microsoft.CodeAnalysis.Diagnostics;
using SymbolicExecution.Analysis.Context;

namespace SymbolicExecution.Analysis;

public interface IHandler<in T>
{
	bool CanHandle(T node);
	SymbolicAnalysisContext Handle(T node, SymbolicAnalysisContext analysisContext, CodeBlockAnalysisContext codeBlockContext);
}