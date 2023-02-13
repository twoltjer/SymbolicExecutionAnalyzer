namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseObjectCreationExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction, IBaseObjectCreationExpressionSyntaxAbstraction
{
	protected BaseObjectCreationExpressionSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? type
		) : base(children, symbol, location, type)
	{
	}
}