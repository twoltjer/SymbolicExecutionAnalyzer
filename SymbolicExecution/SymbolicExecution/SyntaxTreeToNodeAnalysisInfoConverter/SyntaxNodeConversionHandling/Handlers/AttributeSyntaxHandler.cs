namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodeConversionHandling.Handlers;

public class AttributeSyntaxHandler : SyntaxNodeConversionHandlerBase<AttributeSyntax>
{
	protected override async Task<Result<INodeAnalysisInfo>> ProcessNodeAsync(AttributeSyntax node, CancellationToken token)
	{
		var children = await ProcessChildNodesAsync(node, token);
		if (children.IsFaulted)
			return new Result<INodeAnalysisInfo>(children.ErrorInfo);

		var result = new AttributeNodeAnalysisInfo
		{
			Children = children.Value,
		};
		return new Result<INodeAnalysisInfo>(result);
	}
}