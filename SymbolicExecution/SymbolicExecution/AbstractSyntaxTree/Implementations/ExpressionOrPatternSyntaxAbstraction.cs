namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class ExpressionOrPatternSyntaxAbstraction : CSharpSyntaxNodeAbstraction,
	IExpressionOrPatternSyntaxAbstraction
{
	protected ExpressionOrPatternSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}
}