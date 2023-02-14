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

		var scope = new ConstantValueScope(_constantValue.Value, new TaggedUnion<ITypeSymbol, Type>(_type));
		var isBool = scope.IsExactType(typeof(bool));
		var isInt = scope.IsExactType(typeof(int));
		ObjectInstance result;
		if (isBool)
		{
			result = new BoolInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else if (isInt)
		{
			result = new IntInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else
		{
			return new AnalysisFailure("Literal expressions not a known type", Location);
		}
		return ImmutableArray.Create((result as IObjectInstance, state));
	}
}