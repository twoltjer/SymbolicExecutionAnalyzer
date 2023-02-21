namespace SymbolicExecution;

public abstract class ObjectInstance : IObjectInstance
{
	protected readonly int _referenceId;
	public IValueScope Value { get; }
	public TaggedUnion<ITypeSymbol, Type> Type { get; }
	public Location Location { get; }
	public bool IsExactType(Type type) => Value.IsExactType(type);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(
		IObjectInstance right,
		IAnalysisState state
		);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
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
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> ExclusiveOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LeftShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	public abstract TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> RightShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);

	protected ObjectInstance(TaggedUnion<ITypeSymbol, Type> type, Location location, IValueScope value, int referenceId)
	{
		_referenceId = referenceId;
		Type = type;
		Location = location;
		Value = value;
	}

	private static int _nextReferenceId;
	public static int GetNextReferenceId() => _nextReferenceId++;
}

public class ReferenceTypeInstance : ObjectInstance, IReferenceTypeInstance
{
	public ReferenceTypeInstance(TaggedUnion<ITypeSymbol, Type> type, Location location, IValueScope value, int referenceId) : base(type, location, value, referenceId)
	{
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(
		IObjectInstance right,
		IAnalysisState state
		)
	{
		return new AnalysisFailure("Reference type equality is not implemented", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type comparison is not implemented", Location);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type comparison is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type logical and is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type logical or is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type comparison is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type comparison is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> NotEqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type equality is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> AddOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type addition is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> SubtractOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type subtraction is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> MultiplyOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type multiplication is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> DivideOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type division is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> ModuloOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type modulo is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type bitwise and is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type bitwise or is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> ExclusiveOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type exclusive or is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LeftShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type left shift is not implemented", Location);
	}
	
	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> RightShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return new AnalysisFailure("Reference type right shift is not implemented", Location);
	}
}

public abstract class ValueTypeInstance : ObjectInstance, IValueTypeInstance
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

public class PrimitiveValueInstance : ValueTypeInstance, IPrimitiveTypeInstance
{
	public PrimitiveValueInstance(TaggedUnion<ITypeSymbol, Type> type, Location location, IValueScope value, int referenceId) : base(type, location, value, referenceId)
	{
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(
		IObjectInstance right,
		IAnalysisState state
		)
	{
		if (right is not IPrimitiveTypeInstance primitive)
		{
			return new AnalysisFailure("Cannot compare primitive type to non-primitive type", Location);
		}

		var newPrimitiveValueScope =  Value.EqualsOperator(primitive.Value, state);
		if (!newPrimitiveValueScope.IsT1)
		{
			return newPrimitiveValueScope.T2Value;
		}
		else
		{
			var results = new List<(IObjectInstance, IAnalysisState)>();
			foreach (var (primitiveValueScope, newState) in newPrimitiveValueScope.T1Value)
			{
				results.Add((new PrimitiveValueInstance(Type, Location, primitiveValueScope, GetNextReferenceId()), newState));
			}
			return results;
		}
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		if (right is not IPrimitiveTypeInstance primitive)
		{
			return new AnalysisFailure("Cannot compare primitive type to non-primitive type", Location);
		}
		
		var newPrimitiveValueScope = Value.GreaterThanOperator(primitive.Value, state);
		if (!newPrimitiveValueScope.IsT1)
		{
			return newPrimitiveValueScope.T2Value;
		}
		else
		{
			var results = new List<(IObjectInstance, IAnalysisState)>();
			foreach (var (primitiveValueScope, newState) in newPrimitiveValueScope.T1Value)
			{
				results.Add((new PrimitiveValueInstance(Type, Location, primitiveValueScope, GetNextReferenceId()), newState));
			}
			return results;
		}
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		if (right is not IPrimitiveTypeInstance primitive)
		{
			return new AnalysisFailure("Cannot compare primitive type to non-primitive type", Location);
		}
		
		return Value.LessThanOperator(primitive.Value, state);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> NotEqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> AddOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> SubtractOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> MultiplyOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> DivideOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> ModuloOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> ExclusiveOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LeftShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> RightShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		throw new NotImplementedException();
	}
}


