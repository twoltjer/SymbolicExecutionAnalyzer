namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodeConversionHandling.Handlers;

public class MethodDeclarationSyntaxHandler : SyntaxNodeConversionHandlerBase<MethodDeclarationSyntax>
{
	protected override async Task<Result<INodeAnalysisInfo>> ProcessNodeAsync(MethodDeclarationSyntax node, CancellationToken token)
	{
		var children = await ProcessChildNodesAsync(node, token);
		if (children.IsFaulted)
			return new Result<INodeAnalysisInfo>(children.ErrorInfo);

		var result = new MethodDeclarationNodeAnalysisInfo
		{
			Children = children.Value,
		};
		return new Result<INodeAnalysisInfo>(result);
	}
}