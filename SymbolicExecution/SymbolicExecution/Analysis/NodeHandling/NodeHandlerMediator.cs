using SymbolicExecution.Analysis.NodeHandling.NodeHandlers;

namespace SymbolicExecution.Analysis.NodeHandling
{
	public sealed class NodeHandlerMediator : HandlerMediatorBase<SyntaxNode>
	{
		private NodeHandlerMediator(params IHandler<SyntaxNode>[] blockSyntaxHandler) : base(blockSyntaxHandler)
		{
		}

		public static NodeHandlerMediator Instance { get; } = new NodeHandlerMediator(
			new BlockSyntaxHandler(),
			new ThrowStatementSyntaxHandler(),
			new IfStatementSyntaxHandler(),
			new LocalDeclarationStatementSyntaxHandler(),
			new AttributeListSyntaxHandler(),
			new PredefinedTypeSyntaxHandler(),
			new ParameterListSyntaxHandler(),
			new TupleTypeSyntaxHandler(),
			new ExpressionStatementSyntaxHandler()
			);
	}
}