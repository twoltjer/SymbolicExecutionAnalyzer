using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SymbolicExecution.Analysis.ExpressionSyntaxHandling;

public class ExpressionSyntaxHandlerMediator
{
	public IExpressionSyntaxHandler[] Handlers { get; }

	private ExpressionSyntaxHandlerMediator(params IExpressionSyntaxHandler[] handlers)
	{
		Handlers = handlers;
	}

	public SymbolicAnalysisContext Handle(ExpressionSyntax expressionSyntax, SymbolicAnalysisContext analysisContext, CodeBlockAnalysisContext codeBlockContext)
	{
		foreach (var handler in Handlers)
		{
			if (!handler.CanHandle(expressionSyntax))
				continue;

			return handler.Handle(expressionSyntax, analysisContext, codeBlockContext);
		}
		Debug.Fail("No handler found for node " + expressionSyntax.GetType().Name);
		return analysisContext;
	}

	public static ExpressionSyntaxHandlerMediator Instance { get; } = new ExpressionSyntaxHandlerMediator(
		);
}