namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodeConversionHandling.Handlers;

public class PredefinedTypeSyntaxHandler : SyntaxNodeConversionHandlerBase<PredefinedTypeSyntax>
{
	protected override async Task<Result<INodeAnalysisInfo>> ProcessNodeAsync(PredefinedTypeSyntax node, CancellationToken token)
	{
		var children = await ProcessChildNodesAsync(node, token);
		if (children.IsFaulted)
			return new Result<INodeAnalysisInfo>(children.ErrorInfo);

		var result = new PredefinedTypeNodeAnalysisInfo
		{
			Children = children.Value,
		};
		return new Result<INodeAnalysisInfo>(result);
	}
}