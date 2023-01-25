namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class NameSyntaxAbstraction : TypeSyntaxAbstraction, INameSyntaxAbstraction
{
	public NameSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}