using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using SymbolicExecution.Analysis.NodeHandling.NodeHandlers;

namespace SymbolicExecution.Analysis.NodeHandling
{
	public class NodeHandlerMediator
	{
		public INodeHandler[] Handlers { get; }

		private NodeHandlerMediator(params INodeHandler[] handlers)
		{
			Handlers = handlers;
		}

		public SymbolicAnalysisContext Handle(SyntaxNode node, SymbolicAnalysisContext analysisContext, CodeBlockAnalysisContext codeBlockContext)
		{
			foreach (var handler in Handlers)
			{
				if (!handler.CanHandle(node))
					continue;

				return handler.Handle(node, analysisContext, codeBlockContext);
			}
			Debug.Fail("No handler found for node " + node.GetType().Name);
			return analysisContext;
		}

		public static NodeHandlerMediator Instance { get; } = new NodeHandlerMediator(
			new BlockSyntaxHandler(),
			new ThrowStatementSyntaxHandler(),
			new IfStatementSyntaxHandler(),
			new LocalDeclarationStatementSyntaxHandler(),
			new AttributeListSyntaxHandler(),
			new PredefinedTypeSyntaxHandler(),
			new ParameterListSyntaxHandler()
			);
	}
}