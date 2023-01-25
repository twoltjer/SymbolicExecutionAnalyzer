namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class TypeSyntaxAbstraction : ExpressionSyntaxAbstraction, ITypeSyntaxAbstraction
{
	public TypeSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}