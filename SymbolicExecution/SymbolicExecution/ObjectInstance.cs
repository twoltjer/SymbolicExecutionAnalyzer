namespace SymbolicExecution;

public class ObjectInstance : IObjectInstance
{
	private readonly int _referenceId;
	public IValueScope Value { get; }
	public TaggedUnion<ITypeSymbol, Type> Type { get; }
	public Location Location { get; }
	public bool IsExactType(Type type) => Value.IsExactType(type);
	public virtual TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
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

		if (_referenceId != rightObject._referenceId)
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

	private static int _nextReferenceId;
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

public class BoolInstance : PrimitiveInstance<bool>, IBoolInstance
{
	public BoolInstance(Location location, IValueScope value, int referenceId) :
		base(location, value, referenceId)
	{
	}

	public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<bool> converted, out AnalysisFailure? analysisFailure)
	{
		analysisFailure = null;
		if (value is BoolInstance boolInstance)
		{
			converted = boolInstance;
			return true;
		}
		else
		{
			converted = default!;
			return false;
		}
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		if (right is not IPrimitiveInstance<bool> rightBool)
		{
			if (TryConvert(right, out var rightConverted, out var analysisFailure))
			{
				rightBool = rightConverted;
			}
			else if (analysisFailure.HasValue)
			{
				return analysisFailure.Value;
			}
			else if (attemptReverseConversion && right is IPrimitiveInstance rightPrim)
			{
				return rightPrim.LogicalAndOperator(this, state, false);
			}
			else
				return new AnalysisFailure("Right is not a bool", Location);
		}

		if (Value is not ConstantValueScope leftConstant)
			return new AnalysisFailure("Left is not a constant", Location);

		if (rightBool.Value is not ConstantValueScope rightConstant)
			return new AnalysisFailure("Right is not a constant", Location);

		var boolInstance = new BoolInstance(Location, new ConstantValueScope((bool)leftConstant.Value! && (bool)rightConstant.Value!, typeof(bool)), GetNextReferenceId());
		return ImmutableArray.Create((boolInstance as IObjectInstance, state));
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		if (right is not IPrimitiveInstance<bool> rightBool)
		{
			if (TryConvert(right, out var rightConverted, out var analysisFailure))
			{
				rightBool = rightConverted;
			}
			else if (analysisFailure.HasValue)
			{
				return analysisFailure.Value;
			}
			else if (attemptReverseConversion && right is IPrimitiveInstance rightPrim)
			{
				return rightPrim.LogicalOrOperator(this, state, false);
			}
			else
				return new AnalysisFailure("Right is not a bool", Location);
		}

		if (Value is not ConstantValueScope leftConstant)
			return new AnalysisFailure("Left is not a constant", Location);

		if (rightBool.Value is not ConstantValueScope rightConstant)
			return new AnalysisFailure("Right is not a constant", Location);

		var boolInstance = new BoolInstance(Location, new ConstantValueScope((bool)leftConstant.Value! || (bool)rightConstant.Value!, typeof(bool)), GetNextReferenceId());
		return ImmutableArray.Create((boolInstance as IObjectInstance, state));
	}
}

public interface IPrimitiveInstance : IValueTypeInstance
{
}

public interface IPrimitiveInstance<T> : IPrimitiveInstance where T : struct, IEquatable<T>, IComparable<T>
{
	bool TryConvert(IObjectInstance value, out IPrimitiveInstance<T> converted, out AnalysisFailure? analysisFailure);
}

public abstract class PrimitiveInstance<T> : ValueTypeInstance, IPrimitiveInstance<T> where T : struct, IEquatable<T>, IComparable<T>
{
	protected PrimitiveInstance(Location location, IValueScope value, int referenceId) :
		base(typeof(T), location, value, referenceId)
	{
	}

	public abstract bool TryConvert(IObjectInstance value, out IPrimitiveInstance<T> converted, out AnalysisFailure? analysisFailure);


	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return ComparisonOperator(
			right,
			state,
			attemptReverseConversion,
			(rightPrimitive, analysisState) => rightPrimitive.EqualsOperator(
				this,
				analysisState,
				attemptReverseConversion: false
				),
			(a, b) => a.Equals(b)
			);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> NotEqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return ComparisonOperator(
			right,
			state,
			attemptReverseConversion,
			(rightPrimitive, analysisState) => rightPrimitive.NotEqualsOperator(
				this,
				analysisState,
				attemptReverseConversion: false
				),
			(a, b) => !a.Equals(b)
			);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return ComparisonOperator(
			right,
			state,
			attemptReverseConversion,
			(rightPrimitive, analysisState) => rightPrimitive.LessThanOperator(
				this,
				analysisState,
				attemptReverseConversion: false
				),
			(a, b) => a.CompareTo(b) > 0
			);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return ComparisonOperator(
			right,
			state,
			attemptReverseConversion,
			(rightPrimitive, analysisState) => rightPrimitive.GreaterThanOperator(
				this,
				analysisState,
				attemptReverseConversion: false
				),
			(a, b) => a.CompareTo(b) < 0
			);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOrEqualOperator(
		IObjectInstance right,
		IAnalysisState state,
		bool attemptReverseConversion
		)
	{
		return ComparisonOperator(
			right,
			state,
			attemptReverseConversion,
			(rightPrimitive, analysisState) => rightPrimitive.LessThanOrEqualOperator(
				this,
				analysisState,
				attemptReverseConversion: false
				),
			(a, b) => a.CompareTo(b) >= 0
			);
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOrEqualOperator(
		IObjectInstance right,
		IAnalysisState state,
		bool attemptReverseConversion
		)
	{
		return ComparisonOperator(
			right,
			state,
			attemptReverseConversion,
			(rightPrimitive, analysisState) => rightPrimitive.GreaterThanOrEqualOperator(
				this,
				analysisState,
				attemptReverseConversion: false
				),
			(a, b) => a.CompareTo(b) <= 0
			);
	}

	private TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> ComparisonOperator(
		IObjectInstance right,
		IAnalysisState state,
		bool attemptReverseConversion,
		Func<IObjectInstance, IAnalysisState,
			TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure>> reverse,
		Func<T, T, bool> comparison
		)
	{
		if (right is not IPrimitiveInstance<T> rightPrimT)
		{
			if (TryConvert(right, out var rightConverted, out var analysisFailure))
			{
				rightPrimT = rightConverted;
			}
			else if (analysisFailure.HasValue)
			{
				return analysisFailure.Value;
			}
			else if (attemptReverseConversion && right is IPrimitiveInstance rightPrim)
			{
				return reverse(rightPrim, state);
			}
			else
				return new AnalysisFailure("Right is not an instance of the same type and cannot be implicitly converted", Location);
		}

		if (Value is not ConstantValueScope valueScope)
			return new AnalysisFailure("Value is not a constant value scope", Location);

		if (rightPrimT.Value is not ConstantValueScope rightValueScope)
			return new AnalysisFailure("Right value is not a constant value scope", Location);

		if (valueScope.Value is not T value)
		{
			return new AnalysisFailure("Value is not a T", Location);
		}

		if (rightValueScope.Value is not T rightValue)
		{
			return new AnalysisFailure("Right value is not a T", Location);
		}

		var boolInstance = new BoolInstance(Location, new ConstantValueScope(comparison(value, rightValue), typeof(bool)), GetNextReferenceId());
		return ImmutableArray.Create((boolInstance as IObjectInstance, state));
	}
}

public class IntInstance : PrimitiveInstance<int>, IIntInstance
{
	public IntInstance(Location location, IValueScope value, int referenceId) :
		base(location, value, referenceId)
	{
	}

	public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<int> converted, out AnalysisFailure? analysisFailure)
	{
		converted = default!;
		analysisFailure = new AnalysisFailure("Cannot convert value to int", Location);
		return false;
	}

	public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> AddOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
	{
		return ArithmeticOperator(
			right,
			state,
			attemptReverseConversion,
			(rightPrimitive, analysisState) => rightPrimitive.AddOperator(
				this,
				analysisState,
				attemptReverseConversion: false
				),
			(a, b) => a + b
			);
	}

	private TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> ArithmeticOperator(
		IObjectInstance right,
		IAnalysisState state,
		bool attemptReverseConversion,
		Func<IObjectInstance, IAnalysisState,
			TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure>> reverse,
		Func<int, int, int> operation
		)
	{
		if (right is not IPrimitiveInstance<int> rightPrimInt)
		{
			if (TryConvert(right, out var rightConverted, out var analysisFailure))
			{
				rightPrimInt = rightConverted;
			}
			else if (analysisFailure.HasValue)
			{
				return analysisFailure.Value;
			}
			else if (attemptReverseConversion && right is IPrimitiveInstance rightPrim)
			{
				return reverse(rightPrim, state);
			}
			else
				return new AnalysisFailure("Right is not an instance of the same type and cannot be implicitly converted", Location);
		}

		if (Value is not ConstantValueScope valueScope)
			return new AnalysisFailure("Value is not a constant value scope", Location);

		if (rightPrimInt.Value is not ConstantValueScope rightValueScope)
			return new AnalysisFailure("Right value is not a constant value scope", Location);

		if (valueScope.Value is not int value)
		{
			return new AnalysisFailure("Value is not an int", Location);
		}

		if (rightValueScope.Value is not int rightValue)
		{
			return new AnalysisFailure("Right value is not an int", Location);
		}

		// Determine if we may overflow
		if (operation(value, rightValue) < int.MinValue)
		{
			return new AnalysisFailure("Arithmetic operation may overflow", Location);
		}

		var intInstance = new IntInstance(Location, new ConstantValueScope(operation(value, rightValue), typeof(int)), GetNextReferenceId());
		return ImmutableArray.Create((intInstance as IObjectInstance, state));
	}
}

public class LongInstance : PrimitiveInstance<long>, ILongInstance
{
	public LongInstance(Location location, IValueScope value, int referenceId) :
		base(location, value, referenceId)
	{
	}

	public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<long> converted, out AnalysisFailure? analysisFailure)
	{
		if (value is IIntInstance integerValue)
		{
			if (integerValue.Value is ConstantValueScope constantValue)
			{
				if (constantValue.Value is not int intValue)
				{
					converted = default!;
					analysisFailure = new AnalysisFailure("Expected IntInstance with constant value to have an int value", Location);
					return false;
				}

				converted = new LongInstance(Location, new ConstantValueScope((long)intValue, typeof(long)), GetNextReferenceId());
				analysisFailure = default;
				return true;
			}
		}
		converted = default!;
		analysisFailure = new AnalysisFailure("Cannot convert value to long", Location);
		return false;
	}
}

public class ShortInstance : PrimitiveInstance<short>, IShortInstance
{
	public ShortInstance(Location location, IValueScope value, int referenceId) :
		base(location, value, referenceId)
	{
	}

	public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<short> converted, out AnalysisFailure? analysisFailure)
	{
		converted = default!;
		analysisFailure = new AnalysisFailure("Cannot convert value to short", Location);
		return false;
	}
}

public class ByteInstance : PrimitiveInstance<byte>, IByteInstance
{
	public ByteInstance(Location location, IValueScope value, int referenceId) :
		base(location, value, referenceId)
	{
	}

	public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<byte> converted, out AnalysisFailure? analysisFailure)
	{
		converted = default!;
		analysisFailure = new AnalysisFailure("Cannot convert value to byte", Location);
		return false;
	}
}

public class SByteInstance : PrimitiveInstance<sbyte>, ISByteInstance
{
	public SByteInstance(Location location, IValueScope value, int referenceId) :
		base(location, value, referenceId)
	{
	}

	public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<sbyte> converted, out AnalysisFailure? analysisFailure)
	{
		converted = default!;
		analysisFailure = new AnalysisFailure("Cannot convert value to sbyte", Location);
		return false;
	}
}

public class UIntInstance : PrimitiveInstance<uint>, IUIntInstance
{
	public UIntInstance(Location location, IValueScope value, int referenceId) :
		base(location, value, referenceId)
	{
	}

	public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<uint> converted, out AnalysisFailure? analysisFailure)
	{
		converted = default!;
		analysisFailure = new AnalysisFailure("Cannot convert value to uint", Location);
		return false;
	}
}

public class ULongInstance : PrimitiveInstance<ulong>, IULongInstance
{
	public ULongInstance(Location location, IValueScope value, int referenceId) :
		base(location, value, referenceId)
	{
	}

	public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<ulong> converted, out AnalysisFailure? analysisFailure)
	{
		converted = default!;
		analysisFailure = new AnalysisFailure("Cannot convert value to ulong", Location);
		return false;
	}
}

public class UShortInstance : PrimitiveInstance<ushort>, IUShortInstance
{
	public UShortInstance(Location location, IValueScope value, int referenceId) :
		base(location, value, referenceId)
	{
	}

	public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<ushort> converted, out AnalysisFailure? analysisFailure)
	{
		converted = default!;
		analysisFailure = new AnalysisFailure("Cannot convert value to ushort", Location);
		return false;
	}
}

public class CharInstance : PrimitiveInstance<char>, ICharInstance
{
	public CharInstance(Location location, IValueScope value, int referenceId) :
		base(location, value, referenceId)
	{
	}

	public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<char> converted, out AnalysisFailure? analysisFailure)
	{
		converted = default!;
		analysisFailure = new AnalysisFailure("Cannot convert value to char", Location);
		return false;
	}
}

public class FloatInstance : PrimitiveInstance<float>, IFloatInstance
{
	public FloatInstance(Location location, IValueScope value, int referenceId) :
		base(location, value, referenceId)
	{
	}

	public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<float> converted, out AnalysisFailure? analysisFailure)
	{
		converted = default!;
		analysisFailure = new AnalysisFailure("Cannot convert value to float", Location);
		return false;
	}
}

public class DoubleInstance : PrimitiveInstance<double>, IDoubleInstance
{
	public DoubleInstance(Location location, IValueScope value, int referenceId) :
		base(location, value, referenceId)
	{
	}

	public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<double> converted, out AnalysisFailure? analysisFailure)
	{
		converted = default!;
		analysisFailure = new AnalysisFailure("Cannot convert value to double", Location);
		return false;
	}
}

public class DecimalInstance : PrimitiveInstance<decimal>, IDecimalInstance
{
	public DecimalInstance(Location location, IValueScope value, int referenceId) :
		base(location, value, referenceId)
	{
	}

	public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<decimal> converted, out AnalysisFailure? analysisFailure)
	{
		converted = default!;
		analysisFailure = new AnalysisFailure("Cannot convert value to decimal", Location);
		return false;
	}
}

