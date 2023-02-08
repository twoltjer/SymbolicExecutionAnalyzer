namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class ExpressionSyntaxAbstraction : ExpressionOrPatternSyntaxAbstraction, IExpressionSyntaxAbstraction
{
	protected readonly ITypeSymbol? _actualTypeSymbol;
	protected readonly ITypeSymbol? _convertedTypeSymbol;

	protected ExpressionSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? actualTypeSymbol,
		ITypeSymbol? convertedTypeSymbol
		) : base(children, symbol, location)
	{
		_actualTypeSymbol = actualTypeSymbol;
		_convertedTypeSymbol = convertedTypeSymbol;
	}
}