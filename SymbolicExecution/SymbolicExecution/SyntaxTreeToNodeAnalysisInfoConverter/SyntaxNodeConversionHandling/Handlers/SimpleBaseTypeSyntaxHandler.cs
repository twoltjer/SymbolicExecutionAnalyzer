namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodeConversionHandling.Handlers;

public class SimpleBaseTypeSyntaxHandler : SyntaxNodeConversionHandlerBase<SimpleBaseTypeSyntax>
{
	protected override async Task<Result<INodeAnalysisInfo>> ProcessNodeAsync(SimpleBaseTypeSyntax node, CancellationToken token)
	{
		var children = await ProcessChildNodesAsync(node, token);
		if (children.IsFaulted)
			return new Result<INodeAnalysisInfo>(children.ErrorInfo);

		var result = new SimpleBaseTypeNodeAnalysisInfo
		{
			Children = children.Value,
		};
		return new Result<INodeAnalysisInfo>(result);
	}
}