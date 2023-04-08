namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class LiteralExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction, ILiteralExpressionSyntaxAbstraction
{
	private readonly Optional<object?> _constantValue;

	public LiteralExpressionSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		Optional<object?> constantValue,
		ITypeSymbol? actualTypeSymbol,
		ITypeSymbol? convertedTypeSymbol
		) : base(children, symbol, location, actualTypeSymbol, convertedTypeSymbol)
	{
		_constantValue = constantValue;
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze literal expressions", Location);
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		if (!_constantValue.HasValue)
			return new AnalysisFailure("Cannot analyze literal expressions without constant values", Location);

		if (_actualTypeSymbol == null)
			return new AnalysisFailure("Cannot analyze literal expressions without actual type symbols", Location);

		if (_convertedTypeSymbol == null)
			return new AnalysisFailure("Cannot analyze literal expressions without converted type symbols", Location);

		var result = new ObjectInstance(
			new TaggedUnion<ITypeSymbol, Type>(_actualTypeSymbol),
			new TaggedUnion<ITypeSymbol, Type>(_convertedTypeSymbol),
			Location,
			new ConstantValueScope(_constantValue.Value, new TaggedUnion<ITypeSymbol, Type>(_actualTypeSymbol))
			);
		return ImmutableArray.Create((result as IObjectInstance, state));
	}
}