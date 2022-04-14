using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SymbolicExecution.Analysis.NodeHandling
{
	public abstract class NoopNodeHandlerBase<T> : NodeHandlerBase<T> where T : SyntaxNode
	{
		protected override SymbolicAnalysisContext ProcessNode(
			T node,
			SymbolicAnalysisContext analysisContext,
			CodeBlockAnalysisContext codeBlockContext
			)
		{
			return analysisContext;
		}
	}
}