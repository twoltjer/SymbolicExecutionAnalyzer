namespace SymbolicExecution;

public static class ExpressionSyntaxExtensions
{
	public static Result<ExpressionSyntax> NegateBooleanExpression(this ExpressionSyntax syntax)
	{
		return new Result<ExpressionSyntax>(new AnalysisErrorInfo("Cannot negate (yet)!", syntax.GetLocation()));
	}
}