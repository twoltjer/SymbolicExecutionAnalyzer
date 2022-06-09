// using SymbolicExecution.Architecture.Handling;
//
// namespace SymbolicExecution.Analysis.NodeHandling;
//
// public sealed class NodeHandlerMediator : HandlerMediatorBase<SyntaxNode>
// {
// 	private NodeHandlerMediator(params IHandler<SyntaxNode>[] blockSyntaxHandler) : base(blockSyntaxHandler)
// 	{
// 	}
//
// 	public static NodeHandlerMediator Instance { get; } = new NodeHandlerMediator(
// 		new BlockSyntaxHandler(),
// 		new ThrowStatementSyntaxHandler(),
// 		new IfStatementSyntaxHandler(),
// 		new LocalDeclarationStatementSyntaxHandler(),
// 		new AttributeListSyntaxHandler(),
// 		new PredefinedTypeSyntaxHandler(),
// 		new ParameterListSyntaxHandler(),
// 		new TupleTypeSyntaxHandler(),
// 		new ExpressionStatementSyntaxHandler()
// 		);
//
// 	protected override AnalysisErrorInfo HandleUnhandledValue(SyntaxNode value)
// 	{
// 		return new AnalysisErrorInfo("Unhandled syntax node: {value}", value.GetLocation());
// 	}
// }