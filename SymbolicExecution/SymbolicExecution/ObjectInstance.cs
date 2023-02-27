namespace SymbolicExecution;

public abstract class ObjectInstance : IObjectInstance
{
	public int Reference { get; }
	public IValueScope ValueScope { get; }
	public TaggedUnion<ITypeSymbol, Type> Type { get; }
	public Location Location { get; }
	public bool IsType(Type type) => Type.Match(
			t => t.Name == type.Name && t.ContainingNamespace.ToString() == type.Namespace,
			t => t == type
			);

	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalXorOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> NotEqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> AddOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> SubtractOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> MultiplyOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> DivideOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> ModuloOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseXorOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LeftShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> RightShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract IObjectInstance WithValueScope(IValueScope valueScope);

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
}

public interface IReferenceTypeInstance : IObjectInstance
{
}

public sealed class ReferenceTypeInstance : ObjectInstance, IReferenceTypeInstance
{
	public ReferenceTypeInstance(TaggedUnion<ITypeSymbol, Type> type, Location location, IValueScope value, int reference) : base(type, location, value, reference)
	{
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be compared with the == operator", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be compared with the > operator", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be compared with the < operator", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be compared with the && operator", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be compared with the || operator", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalXorOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be compared with the ^ operator", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be compared with the >= operator", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be compared with the <= operator", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> NotEqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be compared with the != operator", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> AddOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be added", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> SubtractOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be subtracted", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> MultiplyOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be multiplied", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> DivideOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be divided", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> ModuloOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be moduloed", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be bitwise anded", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be bitwise ored", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseXorOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be bitwise xored", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LeftShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be left shifted", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> RightShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference types cannot be right shifted", Location);
	}

	public override IObjectInstance WithValueScope(IValueScope valueScope)
	{
		return new ReferenceTypeInstance(Type, Location, valueScope, Reference);
	}
}