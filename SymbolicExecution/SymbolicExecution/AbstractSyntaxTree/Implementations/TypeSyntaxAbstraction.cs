namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class TypeSyntaxAbstraction : ExpressionSyntaxAbstraction, ITypeSyntaxAbstraction
{
	protected TypeSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? type
		) : base(children, symbol, location, type)
	{
	}
}