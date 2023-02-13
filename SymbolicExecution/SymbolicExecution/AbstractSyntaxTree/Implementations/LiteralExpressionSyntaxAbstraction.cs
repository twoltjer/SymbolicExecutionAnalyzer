namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class LiteralExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction, ILiteralExpressionSyntaxAbstraction
{
	private readonly Optional<object?> _constantValue;

	public LiteralExpressionSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		Optional<object?> constantValue,
		ITypeSymbol? type
		) : base(children, symbol, location, type)
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

		if (_type == null)
			return new AnalysisFailure("Cannot analyze literal expressions without actual type symbols", Location);

		var result = new ObjectInstance(new TaggedUnion<ITypeSymbol, Type>(_type), Location, new ConstantValueScope(_constantValue.Value, _type));
		return ImmutableArray.Create((result as IObjectInstance, state));
	}
}