﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SymbolicExecution.Analysis.Context;

namespace SymbolicExecution.Analysis.NodeHandling.NodeHandlers
{
	public class BlockSyntaxHandler : NodeHandlerBase<BlockSyntax>
	{
		protected override SymbolicAnalysisContext ProcessNode(
			BlockSyntax node,
			SymbolicAnalysisContext analysisContext,
			CodeBlockAnalysisContext codeBlockContext
			)
		{
			foreach (var child in node.ChildNodes())
				analysisContext = NodeHandlerMediator.Instance.Handle(child, analysisContext, codeBlockContext);
			return analysisContext;
		}
	}
}