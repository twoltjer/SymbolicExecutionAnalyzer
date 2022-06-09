namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodeConversionHandling.Handlers;

public class AttributeListSyntaxHandler : SyntaxNodeConversionHandlerBase<AttributeListSyntax>
{
	protected override async Task<Result<INodeAnalysisInfo>> ProcessNodeAsync(AttributeListSyntax node, CancellationToken token)
	{
		var children = await ProcessChildNodesAsync(node, token);
		if (children.IsFaulted)
			return new Result<INodeAnalysisInfo>(children.ErrorInfo);

		var result = new AttributeListNodeAnalysisInfo
		{
			Children = children.Value,
		};
		return new Result<INodeAnalysisInfo>(result);
	}
}