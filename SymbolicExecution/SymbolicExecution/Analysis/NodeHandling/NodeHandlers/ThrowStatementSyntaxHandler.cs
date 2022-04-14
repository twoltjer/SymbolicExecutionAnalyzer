using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SymbolicExecution.Analysis.Context;

namespace SymbolicExecution.Analysis.NodeHandling.NodeHandlers
{
	public class ThrowStatementSyntaxHandler : NodeHandlerBase<ThrowStatementSyntax>
	{
		protected override SymbolicAnalysisContext ProcessNode(
			ThrowStatementSyntax node,
			SymbolicAnalysisContext analysisContext,
			CodeBlockAnalysisContext codeBlockContext
			)
		{
			throw new ExceptionStatementException(node);
		}
	}
}