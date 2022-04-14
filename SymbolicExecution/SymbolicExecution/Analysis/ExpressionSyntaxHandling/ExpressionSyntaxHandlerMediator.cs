using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SymbolicExecution.Analysis.ExpressionSyntaxHandling;

public class ExpressionSyntaxHandlerMediator : HandlerMediatorBase<ExpressionSyntax>
{
	private ExpressionSyntaxHandlerMediator(params IHandler<ExpressionSyntax>[] handlers) : base(handlers)
	{
	}


	public static ExpressionSyntaxHandlerMediator Instance { get; } = new ExpressionSyntaxHandlerMediator(
		);
}