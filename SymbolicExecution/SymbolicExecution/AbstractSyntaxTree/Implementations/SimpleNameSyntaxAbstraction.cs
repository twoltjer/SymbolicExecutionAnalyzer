namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class SimpleNameSyntaxAbstraction : NameSyntaxAbstraction, ISimpleNameSyntaxAbstraction
{
	protected SimpleNameSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}