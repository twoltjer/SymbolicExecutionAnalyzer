namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseParameterListSyntaxAbstraction : CSharpSyntaxNodeAbstraction,
	IBaseParameterListSyntaxAbstraction
{
	protected BaseParameterListSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}
}