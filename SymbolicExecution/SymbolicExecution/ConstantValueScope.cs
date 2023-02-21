namespace SymbolicExecution;

public class ConstantValueScope : IValueScope
{
	public object? Value { get; }
	private readonly TaggedUnion<ITypeSymbol, Type> _type;

	public ConstantValueScope(object? value, TaggedUnion<ITypeSymbol, Type> type)
	{
		Value = value;
		_type = type;
	}

	public bool CanBe(object? value)
	{
		if (Value == null)
		{
			return value == null;
		}

		return Value.Equals(value);
	}

	public bool IsExactType(Type type)
	{
		return _type.Match(
			t => t.Name == type.Name && t.ContainingNamespace.ToString() == type.Namespace,
			t => t == type
			);
	}

	public bool IsAlways(object? value)
	{
		return CanBe(value);
	}


	public bool? IsFloatingPoint => Value switch
	{
		byte _ => false,
		sbyte _ => false,
		short _ => false,
		ushort _ => false,
		int _ => false,
		uint _ => false,
		long _ => false,
		ulong _ => false,
		float _ => true,
		double _ => true,
		decimal _ => false,
		_ => null,
	};

	public enum ConstantType
	{
		Number,
		String,
		Bool,
	}
	
	public ConstantType? Type => Value switch
	{
		byte _ => ConstantType.Number,
		sbyte _ => ConstantType.Number,
		short _ => ConstantType.Number,
		ushort _ => ConstantType.Number,
		int _ => ConstantType.Number,
		uint _ => ConstantType.Number,
		long _ => ConstantType.Number,
		ulong _ => ConstantType.Number,
		float _ => ConstantType.Number,
		double _ => ConstantType.Number,
		decimal _ => ConstantType.Number,
		string _ => ConstantType.String,
		bool _ => ConstantType.Bool,
		_ => null,
	};
	
	public int? Precision => Value switch
	{
		byte _ => 8,
		sbyte _ => 8,
		short _ => 16,
		ushort _ => 16,
		int _ => 32,
		uint _ => 32,
		long _ => 64,
		ulong _ => 64,
		float _ => 32,
		double _ => 64,
		decimal _ => 128,
		_ => null,
	};
	

	public TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> EqualsOperator(IValueScope other, IAnalysisState state, Location location)
	{
		if (other is not ConstantValueScope constantValueScope)
			return new AnalysisFailure("Cannot compare a constant value to a non-constant value", location);

		var leftScope = this;
		var rightScope = constantValueScope;
		if (leftScope.Precision != rightScope.Precision)
		{
			if (!leftScope.Precision.HasValue || !rightScope.Precision.HasValue)
				return new AnalysisFailure("Cannot compare a constant value to a non-constant value", location);

			while (leftScope.Precision.Value > rightScope.Precision.Value)
			{
				// Create an instance of the larger type
				rightScope = rightScope.WithBetterPrecision();
			}
		}

	}
	
	public TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> GreaterThanOperator(IValueScope other, IAnalysisState state, Location location)
	{
		if (other is not ConstantValueScope constantValueScope)
			return new AnalysisFailure("Cannot compare a constant value to a non-constant value", location);

		if (Value is not IComparable comparable)
			return new AnalysisFailure("Cannot compare a non-comparable value", location);

		if (constantValueScope.Value is not IComparable comparable2)
			return new AnalysisFailure("Cannot compare to a non-comparable value", location);

		return new[] { (new ConstantValueScope(comparable.CompareTo(comparable2) > 0, _type) as IValueScope, state) };
	}
}