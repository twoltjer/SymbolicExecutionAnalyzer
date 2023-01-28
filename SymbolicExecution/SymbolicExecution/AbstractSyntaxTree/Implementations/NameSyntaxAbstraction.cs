namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class NameSyntaxAbstraction : TypeSyntaxAbstraction, INameSyntaxAbstraction
{
	public NameSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}