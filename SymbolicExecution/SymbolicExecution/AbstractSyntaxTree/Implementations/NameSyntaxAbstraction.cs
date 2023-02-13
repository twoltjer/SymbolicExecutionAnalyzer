namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class NameSyntaxAbstraction : TypeSyntaxAbstraction, INameSyntaxAbstraction
{
	protected NameSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? type
		) : base(children, symbol, location, type)
	{
	}
}