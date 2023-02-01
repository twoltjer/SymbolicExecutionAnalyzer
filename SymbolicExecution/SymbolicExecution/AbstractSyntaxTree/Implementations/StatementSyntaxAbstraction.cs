namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class StatementSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IStatementSyntaxAbstraction
{
	protected StatementSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}
}