// using SymbolicExecution.Architecture.Handling;
//
// namespace SymbolicExecution.Analysis.ExpressionSyntaxHandling;
//
// public class ExpressionSyntaxHandlerMediator : HandlerMediatorBase<ExpressionSyntax>
// {
// 	private ExpressionSyntaxHandlerMediator(params IHandler<ExpressionSyntax>[] handlers) : base(handlers)
// 	{
// 	}
//
//
// 	public static ExpressionSyntaxHandlerMediator Instance { get; } = new ExpressionSyntaxHandlerMediator(
// 		);
//
// 	protected override AnalysisErrorInfo HandleUnhandledValue(ExpressionSyntax value)
// 	{
// 		return new AnalysisErrorInfo($"Unhandled expression syntax: {value}", value.GetLocation());
// 	}
// }