namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class TypeSyntaxAbstraction : ExpressionSyntaxAbstraction, ITypeSyntaxAbstraction
{
	public TypeSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}