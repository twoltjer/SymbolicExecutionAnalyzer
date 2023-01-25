namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class PredefinedTypeSyntaxAbstraction : TypeSyntaxAbstraction, IPredefinedTypeSyntaxAbstraction
{
	public PredefinedTypeSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}