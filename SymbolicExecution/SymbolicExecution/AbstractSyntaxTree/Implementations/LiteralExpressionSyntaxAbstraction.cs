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

	public override TaggedUnion<ImmutableArray<(int, IAnalysisState)>, AnalysisFailure> GetResults(IAnalysisState state)
	{
		if (!_constantValue.HasValue)
			return new AnalysisFailure("Cannot analyze literal expressions without constant values", Location);

		if (_type == null)
			return new AnalysisFailure("Cannot analyze literal expressions without actual type symbols", Location);

		if (_constantValue.Value is bool boolValue)
		{
			var scope = new ConcreteBoolValueScope(boolValue);
			var instance = new BoolInstance(Location, scope, ObjectInstance.GetNextReferenceId());
			state = state.AddReference(instance.Reference, instance);
			return ImmutableArray.Create((instance.Reference, state));
		}

		if (_constantValue.Value is long longValue)
		{
			var scope = new ConcreteIntValueScope(longValue);
			var instance = new IntInstance(Location, scope, ObjectInstance.GetNextReferenceId());
			state = state.AddReference(instance.Reference, instance);
			return ImmutableArray.Create((instance.Reference, state));
		}

		if (_constantValue.Value is int intValue)
		{
			var scope = new ConcreteIntValueScope(intValue);
			var instance = new IntInstance(Location, scope, ObjectInstance.GetNextReferenceId());
			state = state.AddReference(instance.Reference, instance);
			return ImmutableArray.Create((instance.Reference, state));
		}
		
		return new AnalysisFailure("Cannot analyze literal expressions that are not bools", Location);
	}
}