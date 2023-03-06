using System.Numerics;

namespace SymbolicExecution;

public abstract class ValueScope : IValueScope
{
	private readonly TaggedUnion<ITypeSymbol, Type> _type;

	protected ValueScope(TaggedUnion<ITypeSymbol, Type> type)
	{
		_type = type;
	}

	public bool IsExactType(Type type)
	{
		return _type.Match(
			t => t.Name == type.Name && t.ContainingNamespace.ToString() == type.Namespace,
			t => t == type
			);
	}

	public abstract TaggedUnion<IValueScope, AnalysisFailure> AddConstraint(
		IConstraint constraint,
		Location location,
		SymbolicExecutionState symbolicExecutionState
		);
	public abstract TaggedUnion<bool, AnalysisFailure> GetIsReachable(Location location);
}

public sealed class BlankValueScope : ValueScope
{
	public BlankValueScope(TaggedUnion<ITypeSymbol, Type> type) : base(type)
	{
	}

	public override TaggedUnion<IValueScope, AnalysisFailure> AddConstraint(
		IConstraint constraint,
		Location location,
		SymbolicExecutionState symbolicExecutionState
		)
	{
		return constraint switch
		{
			ExactValueConstraint exactValueConstraint => exactValueConstraint.Value is bool boolValue
				? new ConcreteBoolValueScope(boolValue)
				: new AnalysisFailure(
					"Cannot apply an exact value constraint to a BlankValueScope that is not a bool value.",
					location
					),
			_ => new AnalysisFailure("BlankValueScope does not support the given constraint.", location)
		};
	}

	public override TaggedUnion<bool, AnalysisFailure> GetIsReachable(Location location)
	{
		return true;
	}
}

public abstract class IntValueScope : ValueScope
{
	protected IntValueScope() : base(typeof(int))
	{
	}
}

public sealed class ConcreteIntValueScope : IntValueScope
{
	private readonly BigInteger _value;

	public ConcreteIntValueScope(BigInteger value)
	{
		_value = value;
	}

	public override TaggedUnion<IValueScope, AnalysisFailure> AddConstraint(
		IConstraint constraint,
		Location location,
		SymbolicExecutionState symbolicExecutionState
		)
	{
		return constraint switch
		{
			_ => new AnalysisFailure("ConcreteBoolValueScope does not support the given constraint.", location)
		};
	}

	public override TaggedUnion<bool, AnalysisFailure> GetIsReachable(Location location)
	{
		return true;
	}
}
public abstract class BoolValueScope : ValueScope
{
	protected BoolValueScope() : base(typeof(bool))
	{
	}

	public override TaggedUnion<bool, AnalysisFailure> GetIsReachable(Location location)
	{
		return true;
	}

	public abstract TaggedUnion<BoolValueScope, AnalysisFailure> LogicalInvert(Location location);
}

public sealed class AnyBoolValueScope : BoolValueScope
{
	public override TaggedUnion<IValueScope, AnalysisFailure> AddConstraint(
		IConstraint constraint,
		Location location,
		SymbolicExecutionState symbolicExecutionState
		)
	{
		return constraint switch
		{
			ExactValueConstraint exactValueConstraint => exactValueConstraint.Value is bool boolValue
				? new ConcreteBoolValueScope(boolValue)
				: new AnalysisFailure(
					"Cannot apply an exact value constraint to an AnyBoolValueScope that is not a bool value.",
					location
					),
			_ => new AnalysisFailure("AnyBoolValueScope does not support the given constraint.", location)
		};
	}

	public override TaggedUnion<BoolValueScope, AnalysisFailure> LogicalInvert(Location location)
	{
		return new AnyBoolValueScope();
	}
}

public class DerivedBoolValueScope : BoolValueScope
{
	private readonly int _leftOrOnlyParentRef;
	private readonly int? _rightParentRef;
	private readonly BoolDerivation _derivation;

	public enum BoolDerivation
	{
		Equals,
		LogicalNot,
		GreaterThan,
		LessThan,
		LogicalAnd
	}
	
	private DerivedBoolValueScope(int leftOrOnlyParentRef, int? rightParentRef, BoolDerivation derivation)
	{
		_leftOrOnlyParentRef = leftOrOnlyParentRef;
		_rightParentRef = rightParentRef;
		_derivation = derivation;
	}
	
	public static TaggedUnion<DerivedBoolValueScope, AnalysisFailure> Create(BoolDerivation derivation, int parentRef, int? secondParentRef, Location location)
	{
		if (derivation == BoolDerivation.LogicalNot)
		{
			if (secondParentRef.HasValue)
				return new AnalysisFailure(
					$"Cannot derive a bool value from a {derivation} with more than one parent.",
					location
					);
			return new DerivedBoolValueScope(parentRef, null, derivation);
		}
		else if (derivation == BoolDerivation.Equals)
		{
			if (secondParentRef.HasValue)
				return new AnalysisFailure(
					$"Cannot derive a bool value from a {derivation} with only one parent.",
					location
					);
			return new DerivedBoolValueScope(parentRef, secondParentRef, derivation);
		}
		else if (derivation == BoolDerivation.GreaterThan)
		{
			if (!secondParentRef.HasValue)
				return new AnalysisFailure(
					$"Cannot derive a bool value from a {derivation} with only one parent.",
					location
					);
			return new DerivedBoolValueScope(parentRef, secondParentRef.Value, derivation);
		}
		else if (derivation == BoolDerivation.LessThan)
		{
			if (!secondParentRef.HasValue)
				return new AnalysisFailure(
					$"Cannot derive a bool value from a {derivation} with only one parent.",
					location
					);
			return new DerivedBoolValueScope(parentRef, secondParentRef.Value, derivation);
		}
		else if (derivation == BoolDerivation.LogicalAnd)
		{
			if (!secondParentRef.HasValue)
				return new AnalysisFailure(
					$"Cannot derive a bool value from a {derivation} with only one parent.",
					location
					);
			return new DerivedBoolValueScope(parentRef, secondParentRef.Value, derivation);
		}
		else
		{
			return new AnalysisFailure(
				$"Cannot derive a bool value from a {derivation}.",
				location
				);
		}
	}

	public override TaggedUnion<IValueScope, AnalysisFailure> AddConstraint(
		IConstraint constraint,
		Location location,
		SymbolicExecutionState symbolicExecutionState
		)
	{
		var leftOrOnlyParent = symbolicExecutionState.References[_leftOrOnlyParentRef];
		var leftOrOnlyParentScope = leftOrOnlyParent.ValueScope;

		if (_derivation == BoolDerivation.LogicalNot)
		{
			if (leftOrOnlyParentScope is not BoolValueScope boolValueScope)
				return new AnalysisFailure(
					$"Cannot derive a bool value from a {leftOrOnlyParentScope.GetType().Name}.",
					location
					);
			var bvsOrFail = boolValueScope.LogicalInvert(location);
			if (!bvsOrFail.IsT1)
				return bvsOrFail.T2Value;

			boolValueScope = bvsOrFail.T1Value;
			var boolValueScopeOrFailure = boolValueScope.AddConstraint(constraint, location, symbolicExecutionState);
			return boolValueScopeOrFailure;
		}
	
		return new AnalysisFailure(
			$"Cannot derive a bool value from a {_derivation}.",
			location
			);
	}

	// public override BoolValueScope LogicalInvert()
	// {
	// 	return new DerivedBoolValueScope(_leftOrOnlyParentRef, _derivation == BoolDerivation.Equals ? BoolDerivation.LogicalNot : BoolDerivation.Equals);
	// }
	public override TaggedUnion<BoolValueScope, AnalysisFailure> LogicalInvert(Location location)
	{
		TaggedUnion<BoolDerivation, AnalysisFailure> derivationOrFailure = _derivation switch
		{
			BoolDerivation.Equals => BoolDerivation.LogicalNot,
			BoolDerivation.LogicalNot => BoolDerivation.Equals,
			_ => new AnalysisFailure(
				$"Cannot invert a {_derivation}.",
				Location.None
				)
		};
		if (!derivationOrFailure.IsT1)
			return derivationOrFailure.T2Value;
		
		var derivation = derivationOrFailure.T1Value;
		var result = DerivedBoolValueScope.Create(derivation, _leftOrOnlyParentRef, default, location);
		if (!result.IsT1)
			return result.T2Value;
		
		return result.T1Value;
	}
}

public sealed class ConcreteBoolValueScope : BoolValueScope
{
	private readonly bool _value;

	public ConcreteBoolValueScope(bool value)
	{
		_value = value;
	}

	public override TaggedUnion<IValueScope, AnalysisFailure> AddConstraint(
		IConstraint constraint,
		Location location,
		SymbolicExecutionState symbolicExecutionState
		)
	{
		return constraint switch
		{
			ExactValueConstraint exactValueConstraint => exactValueConstraint.Value is bool boolValue
				? boolValue == _value
					? this
					: new UnreachableValueScope()
				: new AnalysisFailure(
					"Cannot apply an exact value constraint to a ConcreteBoolValueScope that is not a bool value.",
					location
					),
			
			_ => new AnalysisFailure("ConcreteBoolValueScope does not support the given constraint.", location)
		};
	}

	public override TaggedUnion<BoolValueScope, AnalysisFailure> LogicalInvert(Location location)
	{
		return new ConcreteBoolValueScope(!_value);
	}
}

public sealed class UnreachableValueScope : ValueScope
{
	public UnreachableValueScope() : base(typeof(bool))
	{
	}

	public override TaggedUnion<IValueScope, AnalysisFailure> AddConstraint(
		IConstraint constraint,
		Location location,
		SymbolicExecutionState symbolicExecutionState
		)
	{
		return this;
	}

	public override TaggedUnion<bool, AnalysisFailure> GetIsReachable(Location location)
	{
		return false;
	}
}