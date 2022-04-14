using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SymbolicExecution.Analysis.NodeHandling
{
	public interface INodeHandler
	{
		bool CanHandle(SyntaxNode node);
		SymbolicAnalysisContext Handle(SyntaxNode node, SymbolicAnalysisContext analysisContext, CodeBlockAnalysisContext codeBlockContext);
	}
}