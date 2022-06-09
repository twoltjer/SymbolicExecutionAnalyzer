// using SymbolicExecution.Architecture.Handling;
//
// namespace SymbolicExecution.Analysis.ExpressionSyntaxHandling;
//
// public abstract class ExpressionSyntaxHandlerBase<T> : IHandler<ExpressionSyntax> where T : ExpressionSyntax
// {
// 	public bool CanHandle(ExpressionSyntax expressionSyntax) => expressionSyntax is T;
//
// 	public Result<SymbolicAnalysisContext> Handle(
// 		ExpressionSyntax expressionSyntax,
// 		SymbolicAnalysisContext analysisContext,
// 		CodeBlockAnalysisContext codeBlockContext
// 		)
// 	{
// 		return ProcessExpressionSyntax((T)expressionSyntax, analysisContext, codeBlockContext);
// 	}
//
// 	protected abstract Result<SymbolicAnalysisContext> ProcessExpressionSyntax(T node, SymbolicAnalysisContext analysisContext, CodeBlockAnalysisContext codeBlockContext);
// }