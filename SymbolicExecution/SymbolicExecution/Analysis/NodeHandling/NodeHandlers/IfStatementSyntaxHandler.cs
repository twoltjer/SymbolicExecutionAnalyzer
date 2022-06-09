// using System.Collections.Generic;
//
// namespace SymbolicExecution.Analysis.NodeHandling.NodeHandlers;
//
// public class IfStatementSyntaxHandler : NodeHandlerBase<IfStatementSyntax>
// {
// 	protected override Result<SymbolicAnalysisContext> ProcessNode(
// 		IfStatementSyntax node,
// 		SymbolicAnalysisContext analysisContext,
// 		CodeBlockAnalysisContext codeBlockContext
// 		)
// 	{
// 		var condition = node.Condition;
// 		var negatedCondition = condition.NegateBooleanExpression();
// 		if (negatedCondition.IsFaulted)
// 			return new Result<SymbolicAnalysisContext>(negatedCondition.ErrorInfo);
//
// 		var truePaths = analysisContext.ExecutionPaths.Select(x => x.WithCondition(condition));
// 		var falsePaths = analysisContext.ExecutionPaths.Select(x => x.WithCondition(condition));
//
// 		var resultingPaths = new List<ExecutionPath>();
// 		foreach (var path in truePaths)
// 		{
// 			var isValidResult = path.IsValid();
// 			if (isValidResult.IsFaulted)
// 				return new Result<SymbolicAnalysisContext>(isValidResult.ErrorInfo);
//
// 			if (isValidResult.Value)
// 			{
// 				NodeHandlerMediator.Instance.Handle(node.Statement, , codeBlockContext)
// 			}
// 		}
//
// 		return analysisContext;
// 	}
// }