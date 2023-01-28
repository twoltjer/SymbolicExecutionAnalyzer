namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class SimpleNameSyntaxAbstraction : NameSyntaxAbstraction, ISimpleNameSyntaxAbstraction
{
	public SimpleNameSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}