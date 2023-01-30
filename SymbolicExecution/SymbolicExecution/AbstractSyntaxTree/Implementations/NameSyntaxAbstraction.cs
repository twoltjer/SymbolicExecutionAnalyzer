namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class NameSyntaxAbstraction : TypeSyntaxAbstraction, INameSyntaxAbstraction
{
	protected NameSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}