namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodeConversionHandling.Handlers;

public class CompilationUnitSyntaxHandler : SyntaxNodeConversionHandlerBase<CompilationUnitSyntax>
{
	protected override async Task<Result<INodeAnalysisInfo>> ProcessNodeAsync(CompilationUnitSyntax node, CancellationToken token)
	{
		var children = await ProcessChildNodesAsync(node, token);
		if (children.IsFaulted)
			return new Result<INodeAnalysisInfo>(children.ErrorInfo);

		var result = new CompilationUnitNodeAnalysisInfo
		{
			Children = children.Value,
		};
		return new Result<INodeAnalysisInfo>(result);
	}
}