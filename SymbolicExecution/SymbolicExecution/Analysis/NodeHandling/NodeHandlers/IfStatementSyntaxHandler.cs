using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SymbolicExecution.Analysis.Context;

namespace SymbolicExecution.Analysis.NodeHandling.NodeHandlers
{
	public class IfStatementSyntaxHandler : NodeHandlerBase<IfStatementSyntax>
	{
		protected override SymbolicAnalysisContext ProcessNode(
			IfStatementSyntax node,
			SymbolicAnalysisContext analysisContext,
			CodeBlockAnalysisContext codeBlockContext
			)
		{
			var condition = node.Condition;
			var trueContext = analysisContext.WithCondition(condition);
			if (trueContext.CanBeTrue())
			{
				var mutatedContext = NodeHandlerMediator.Instance.Handle(node.Statement, trueContext, codeBlockContext);
				if (trueContext != mutatedContext)
					throw new ValidationFailedException("If-statement context was mutated.");
			}
			if (node.Else != null)
			{
				throw new UnhandledSyntaxException();
				// var falseContext = analysisContext.WithCondition(NegateCondition(condition));
				// if (falseContext.CanBeTrue())
				// {
				// 	var mutatedContext = NodeHandlerMediator.Instance.Handle(node.Statement, falseContext, codeBlockContext);
				// 	if (falseContext != mutatedContext)
				// 		throw new ValidationFailedException("IfStatementSyntax should not mutate the context");
				// }
			}

			return analysisContext;
		}

		//TODO: Come back to this and implement it
		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		private ExpressionSyntax NegateCondition(ExpressionSyntax condition) => throw new UnhandledSyntaxException();
	}
}