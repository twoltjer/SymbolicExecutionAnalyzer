namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodeConversionHandling.Handlers;

public class IdentifierNameSyntaxHandler : SyntaxNodeConversionHandlerBase<IdentifierNameSyntax>
{
	protected override async Task<Result<INodeAnalysisInfo>> ProcessNodeAsync(IdentifierNameSyntax node, CancellationToken token)
	{
		var children = await ProcessChildNodesAsync(node, token);
		if (children.IsFaulted)
			return new Result<INodeAnalysisInfo>(children.ErrorInfo);

		var result = new IdentifierNameNodeAnalysisInfo
		{
			Children = children.Value,
		};
		return new Result<INodeAnalysisInfo>(result);
	}
}