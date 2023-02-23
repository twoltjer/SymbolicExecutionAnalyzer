namespace SymbolicExecution;

public class ObjectInstance : IObjectInstance
{
	public int Reference { get; }
	public IValueScope ValueScope { get; }
	public TaggedUnion<ITypeSymbol, Type> Type { get; }
	public Location Location { get; }
	public bool IsType(Type type) => Type.Match(
			t => t.Name == type.Name && t.ContainingNamespace.ToString() == type.Namespace,
			t => t == type
			);
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		if (right is not ObjectInstance rightObject)
			return new AnalysisFailure("Right is not an object instance", Location);

		if (Reference == rightObject.Reference)
		{
			var boolInstance = new BoolInstance(Location, new ConstantValueScope(true, typeof(bool)), GetNextReferenceId());
			return ImmutableArray.Create((boolInstance as IObjectInstance, state));
		}
		else
			return ImmutableArray<(IObjectInstance, IAnalysisState)>.Empty;
	}
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot compare objects", Location);
	}

	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot compare objects", Location);
	}

	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot perform logical and on objects", Location);
	}

	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot perform logical or on objects", Location);
	}
	
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalXorOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot perform logical xor on objects", Location);
	}
	
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> AddOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot add objects", Location);
	}
	
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> SubtractOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot subtract objects", Location);
	}
	
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> MultiplyOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot multiply objects", Location);
	}
	
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> DivideOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot divide objects", Location);
	}
	
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> ModuloOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot modulo objects", Location);
	}
	
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot perform bitwise and on objects", Location);
	}
	
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot perform bitwise or on objects", Location);
	}
	
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseXorOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot perform bitwise xor on objects", Location);
	}
	
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseNotOperator(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot perform bitwise not on objects", Location);
	}
	
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LeftShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot perform left shift on objects", Location);
	}
	
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> RightShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot perform right shift on objects", Location);
	}
	
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot compare objects", Location);
	}

	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Cannot compare objects", Location);
	}

	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> NotEqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		if (right is not ObjectInstance rightObject)
			return new AnalysisFailure("Right is not an object instance", Location);

		if (Reference != rightObject.Reference)
		{
			var boolInstance = new BoolInstance(Location, new ConstantValueScope(true, typeof(bool)), GetNextReferenceId());
			return ImmutableArray.Create((boolInstance as IObjectInstance, state));
		}
		else
			return ImmutableArray<(IObjectInstance, IAnalysisState)>.Empty;
	}

	public ObjectInstance(TaggedUnion<ITypeSymbol, Type> type, Location location, IValueScope value, int reference)
	{
		Reference = reference;
		Type = type;
		Location = location;
		ValueScope = value;
	}

	private static int _nextReferenceId;
	public static int GetNextReferenceId() => _nextReferenceId++;
}

public interface IPrimitiveInstance<T> : IPrimitiveInstance where T : struct, IEquatable<T>, IComparable<T>
{
	bool TryConvert(IObjectInstance value, out IPrimitiveInstance<T> converted, out AnalysisFailure? analysisFailure);
}