namespace SymbolicExecution;

public class IntInstance : PrimitiveInstance<int>, IIntInstance
{
    public IntInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
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

        if (ValueScope is not ConstantValueScope valueScope)
            return new AnalysisFailure("Value is not a constant value scope", Location);

        if (rightPrimInt.ValueScope is not ConstantValueScope rightValueScope)
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