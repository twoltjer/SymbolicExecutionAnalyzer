using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SymbolicExecution.Analysis.NodeHandling
{
	public abstract class NodeHandlerBase<T> : IHandler<SyntaxNode> where T : SyntaxNode
	{
		public bool CanHandle(SyntaxNode node) => node is T;

		public SymbolicAnalysisContext Handle(
			SyntaxNode node,
			SymbolicAnalysisContext analysisContext,
			CodeBlockAnalysisContext codeBlockContext
			)
		{
			return ProcessNode((T)node, analysisContext, codeBlockContext);
		}

		protected abstract SymbolicAnalysisContext ProcessNode(T node, SymbolicAnalysisContext analysisContext, CodeBlockAnalysisContext codeBlockContext);
	}
}