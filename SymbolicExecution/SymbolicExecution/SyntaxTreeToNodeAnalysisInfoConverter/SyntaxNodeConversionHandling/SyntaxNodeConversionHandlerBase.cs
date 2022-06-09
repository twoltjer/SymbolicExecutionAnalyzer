namespace SymbolicExecution.SyntaxTreeToNodeAnalysisInfoConverter.SyntaxNodeConversionHandling;

public abstract class SyntaxNodeConversionHandlerBase<T> : IHandler<SyntaxNode, Result<INodeAnalysisInfo>> where T : SyntaxNode
{
	public bool CanHandle(SyntaxNode node) => node is T;

	public async Task<Result<INodeAnalysisInfo>> HandleAsync(
		SyntaxNode node,
		CancellationToken token
		)
	{
		return await ProcessNodeAsync((T)node, token);
	}

	protected abstract Task<Result<INodeAnalysisInfo>> ProcessNodeAsync(T node, CancellationToken token);

	protected async Task<Result<INodeAnalysisInfo[]>> ProcessChildNodesAsync(T node, CancellationToken token)
	{
		var children = await Task.WhenAll(
			node.ChildNodes().Select(x => SyntaxNodeConversionHandlerMediator.Instance.HandleAsync(x, token)).ToArray()
			);
		foreach (var child in children)
		{
			if (child.IsFaulted)
				return new Result<INodeAnalysisInfo[]>(child.ErrorInfo);
		}

		return new Result<INodeAnalysisInfo[]>(children.Select(x => x.Value).ToArray());
	}
}