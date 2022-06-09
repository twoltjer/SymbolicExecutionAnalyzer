namespace SymbolicExecution.CodeBlockToAnalysisStructConverter.SyntaxNodeConversionHandling;

public class SyntaxNodeConversionHandlerMediator : HandlerMediatorBase<SyntaxNode, Result<INodeAnalysisInfo>>
{
	private SyntaxNodeConversionHandlerMediator(params IHandler<SyntaxNode, Result<INodeAnalysisInfo>>[] blockSyntaxHandler) : base(
		blockSyntaxHandler
		)
	{
	}

	public static SyntaxNodeConversionHandlerMediator Instance { get; } = new SyntaxNodeConversionHandlerMediator(
		);

	protected override Result<INodeAnalysisInfo> HandleUnhandledValue(SyntaxNode value)
	{
		return new Result<INodeAnalysisInfo>(new AnalysisErrorInfo($"Unhandled syntax node: {value.GetType()}", value.GetLocation()));
	}
}