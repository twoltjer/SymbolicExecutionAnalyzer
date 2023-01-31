namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class ExpressionSyntaxAbstraction : ExpressionOrPatternSyntaxAbstraction, IExpressionSyntaxAbstraction
{
	protected ExpressionSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}
}