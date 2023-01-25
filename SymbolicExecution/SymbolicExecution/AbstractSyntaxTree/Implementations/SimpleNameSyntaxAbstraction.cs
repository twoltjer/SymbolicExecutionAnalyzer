namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class SimpleNameSyntaxAbstraction : NameSyntaxAbstraction, ISimpleNameSyntaxAbstraction
{
	public SimpleNameSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}