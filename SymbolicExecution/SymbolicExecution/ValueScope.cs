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

public abstract class BoolValueScope : ValueScope
{
	protected BoolValueScope() : base(typeof(bool))
	{
	}

	public override TaggedUnion<bool, AnalysisFailure> GetIsReachable(Location location)
	{
		return true;
	}
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
}

public class DerivedBoolValueScope : BoolValueScope
{
	private readonly int _parentRef;
	private readonly BoolDerivation _derivation;

	public enum BoolDerivation
	{
		Equals,
		NotEquals
	}
	
	public DerivedBoolValueScope(int parentRef, BoolDerivation derivation)
	{
		_parentRef = parentRef;
		_derivation = derivation;
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
					"Cannot apply an exact value constraint to a DerivedBoolValueScope that is not a bool value.",
					location
					),
			_ => new AnalysisFailure("DerivedBoolValueScope does not support the given constraint.", location)
		};
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