namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseObjectCreationExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction, IBaseObjectCreationExpressionSyntaxAbstraction
{
	protected BaseObjectCreationExpressionSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? actualTypeSymbol,
		ITypeSymbol? convertedTypeSymbol
		) : base(children, symbol, location, actualTypeSymbol, convertedTypeSymbol)
	{
	}
}