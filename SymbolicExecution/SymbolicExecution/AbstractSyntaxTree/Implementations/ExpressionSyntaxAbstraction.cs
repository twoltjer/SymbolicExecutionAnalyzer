namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class ExpressionSyntaxAbstraction : ExpressionOrPatternSyntaxAbstraction, IExpressionSyntaxAbstraction
{
	protected readonly ITypeSymbol? _type;

	protected ExpressionSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? typeSymbol
		) : base(children, symbol, location)
	{
		_type = typeSymbol;
	}
	
	public abstract TaggedUnion<ImmutableArray<(int, IAnalysisState)>, AnalysisFailure> GetResults(IAnalysisState state);
}