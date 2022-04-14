using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

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
				Debug.Assert(trueContext == mutatedContext, "IfStatementSyntax should not mutate the context");
			}
			if (node.Else != null)
			{
				var falseContext = analysisContext.WithCondition(NegateCondition(condition));
				if (falseContext.CanBeTrue())
				{
					var mutatedContext = NodeHandlerMediator.Instance.Handle(node.Statement, falseContext, codeBlockContext);
					Debug.Assert(falseContext == mutatedContext, "IfStatementSyntax should not mutate the context");
				}
			}

			return analysisContext;
		}

		private ExpressionSyntax NegateCondition(ExpressionSyntax condition)
		{
			Debug.Fail("Not implemented");
			throw new NotImplementedException();
		}
	}
}