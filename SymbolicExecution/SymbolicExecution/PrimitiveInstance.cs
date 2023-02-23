namespace SymbolicExecution;

public abstract class PrimitiveInstance<T> : ValueTypeInstance, IPrimitiveInstance<T> where T : struct, IEquatable<T>, IComparable<T>
{
    protected PrimitiveInstance(Location location, IValueScope value, int reference) :
        base(typeof(T), location, value, reference)
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

        if (ValueScope is not ConstantValueScope valueScope)
            return new AnalysisFailure("Value is not a constant value scope", Location);

        if (rightPrimT.ValueScope is not ConstantValueScope rightValueScope)
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