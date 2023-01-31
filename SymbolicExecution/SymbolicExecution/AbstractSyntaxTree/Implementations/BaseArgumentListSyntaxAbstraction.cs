namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseArgumentListSyntaxAbstraction : CSharpSyntaxNodeAbstraction,
	IBaseArgumentListSyntaxAbstraction
{
	protected BaseArgumentListSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}
}