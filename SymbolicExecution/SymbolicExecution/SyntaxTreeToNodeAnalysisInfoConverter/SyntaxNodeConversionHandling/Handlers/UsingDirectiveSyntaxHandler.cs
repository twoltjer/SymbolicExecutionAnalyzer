namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodeConversionHandling.Handlers;

public class UsingDirectiveSyntaxHandler : SyntaxNodeConversionHandlerBase<UsingDirectiveSyntax>
{
	protected override async Task<Result<INodeAnalysisInfo>> ProcessNodeAsync(UsingDirectiveSyntax node, CancellationToken token)
	{
		var children = await ProcessChildNodesAsync(node, token);
		if (children.IsFaulted)
			return new Result<INodeAnalysisInfo>(children.ErrorInfo);

		var result = new UsingDirectiveNodeAnalysisInfo
		{
			Children = children.Value,
		};
		return new Result<INodeAnalysisInfo>(result);
	}
}