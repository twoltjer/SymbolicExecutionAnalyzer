namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class SimpleNameSyntaxAbstraction : NameSyntaxAbstraction, ISimpleNameSyntaxAbstraction
{
	protected SimpleNameSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? type
		) : base(children, symbol, location, type)
	{
	}
}