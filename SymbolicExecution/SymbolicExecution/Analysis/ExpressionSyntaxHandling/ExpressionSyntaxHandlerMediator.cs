using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SymbolicExecution.Analysis.ExpressionSyntaxHandling;

public class ExpressionSyntaxHandlerMediator : HandlerMediatorBase<ExpressionSyntax>
{
	private ExpressionSyntaxHandlerMediator(params IHandler<ExpressionSyntax>[] handlers) : base(handlers)
	{
	}


	public static ExpressionSyntaxHandlerMediator Instance { get; } = new ExpressionSyntaxHandlerMediator(
		);
}