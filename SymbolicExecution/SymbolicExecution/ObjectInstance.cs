namespace SymbolicExecution;

public class ObjectInstance : IObjectInstance
{
	private readonly int _referenceId;
	public IValueScope Value { get; }
	public TaggedUnion<ITypeSymbol, Type> Type { get; }
	public Location Location { get; }
	public bool IsExactType(Type type) => Value.IsExactType(type);
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(IObjectInstance right, IAnalysisState state)
	{
		if (right is not ObjectInstance rightObject)
			return new AnalysisFailure("Right is not an object instance", Location);

		if (_referenceId == rightObject._referenceId)
		{
			var boolInstance = new BoolInstance(Location, new ConstantValueScope(true, typeof(bool)), GetNextReferenceId());
			return ImmutableArray.Create((boolInstance as IObjectInstance, state));
		}
		else
			return ImmutableArray<(IObjectInstance, IAnalysisState)>.Empty;
	}

	public ObjectInstance(TaggedUnion<ITypeSymbol, Type> type, Location location, IValueScope value, int referenceId)
	{
		_referenceId = referenceId;
		Type = type;
		Location = location;
		Value = value;
	}

	private static int _nextReferenceId = 0;
	public static int GetNextReferenceId() => _nextReferenceId++;
}

public class ValueTypeInstance : ObjectInstance, IValueTypeInstance
{
	public ValueTypeInstance(
		TaggedUnion<ITypeSymbol, Type> type,
		Location location,
		IValueScope value,
		int referenceId
		) : base(type, location, value, referenceId)
	{
	}
}

public class BoolInstance : ValueTypeInstance, IBoolInstance
{
	public BoolInstance(Location location, IValueScope value, int referenceId) :
		base(typeof(bool), location, value, referenceId)
	{
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(IObjectInstance right, IAnalysisState state)
	{
		if (right is not BoolInstance rightBool)
			return new AnalysisFailure("Right is not a bool instance", Location);

		if (Value is not ConstantValueScope valueScope)
			return new AnalysisFailure("Value is not a constant value scope", Location);

		if (rightBool.Value is not ConstantValueScope rightValueScope)
			return new AnalysisFailure("Right value is not a constant value scope", Location);

		if (valueScope.Value is not bool value)
			return new AnalysisFailure("Value is not a bool", Location);

		if (rightValueScope.Value is not bool rightValue)
			return new AnalysisFailure("Right value is not a bool", Location);

		var boolInstance = new BoolInstance(Location, new ConstantValueScope(value == rightValue, typeof(bool)), GetNextReferenceId());
		return ImmutableArray.Create((boolInstance as IObjectInstance, state));
	}
}

public class IntInstance : ValueTypeInstance
{
	public IntInstance(Location location, IValueScope value, int referenceId) :
		base(typeof(int), location, value, referenceId)
	{
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(IObjectInstance right, IAnalysisState state)
	{
		if (right is not IntInstance rightInt)
			return new AnalysisFailure("Right is not an int instance", Location);

		if (Value is not ConstantValueScope valueScope)
			return new AnalysisFailure("Value is not a constant value scope", Location);

		if (rightInt.Value is not ConstantValueScope rightValueScope)
			return new AnalysisFailure("Right value is not a constant value scope", Location);

		if (valueScope.Value is not int value)
			return new AnalysisFailure("Value is not an int", Location);

		if (rightValueScope.Value is not int rightValue)
			return new AnalysisFailure("Right value is not an int", Location);

		var boolInstance = new BoolInstance(Location, new ConstantValueScope(value == rightValue, typeof(bool)), GetNextReferenceId());
		return ImmutableArray.Create((boolInstance as IObjectInstance, state));
	}
}