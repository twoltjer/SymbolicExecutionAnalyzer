// namespace SymbolicExecution.Analysis.NodeHandling.NodeHandlers;
//
// public class BlockSyntaxHandler : NodeHandlerBase<BlockSyntax>
// {
// 	protected override Result<SymbolicAnalysisContext> ProcessNode(
// 		BlockSyntax node,
// 		SymbolicAnalysisContext analysisContext,
// 		CodeBlockAnalysisContext codeBlockContext
// 		)
// 	{
// 		foreach (var child in node.ChildNodes())
// 		{
// 			var result = NodeHandlerMediator.Instance.Handle(child, analysisContext, codeBlockContext);
// 			if (result.IsFaulted)
// 				return result;
// 			analysisContext = result.Value;
// 		}
// 		return new Result<SymbolicAnalysisContext>(analysisContext);
// 	}
// }