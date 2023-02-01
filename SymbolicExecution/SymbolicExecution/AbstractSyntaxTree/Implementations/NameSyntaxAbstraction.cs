namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class NameSyntaxAbstraction : TypeSyntaxAbstraction, INameSyntaxAbstraction
{
	protected NameSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}
}