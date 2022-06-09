namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodeConversionHandling.Handlers;

public class ClassDeclarationSyntaxHandler : SyntaxNodeConversionHandlerBase<ClassDeclarationSyntax>
{
	protected override async Task<Result<INodeAnalysisInfo>> ProcessNodeAsync(ClassDeclarationSyntax node, CancellationToken token)
	{
		var children = await ProcessChildNodesAsync(node, token);
		if (children.IsFaulted)
			return new Result<INodeAnalysisInfo>(children.ErrorInfo);

		var result = new ClassDeclarationNodeAnalysisInfo
		{
			Children = children.Value,
		};
		return new Result<INodeAnalysisInfo>(result);
	}
}