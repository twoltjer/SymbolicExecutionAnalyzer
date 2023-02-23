namespace SymbolicExecution;

public class BoolInstance : PrimitiveInstance<bool>, IBoolInstance
{
    public BoolInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
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

        if (ValueScope is not ConstantValueScope leftConstant)
            return new AnalysisFailure("Left is not a constant", Location);

        if (rightBool.ValueScope is not ConstantValueScope rightConstant)
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

        if (ValueScope is not ConstantValueScope leftConstant)
            return new AnalysisFailure("Left is not a constant", Location);

        if (rightBool.ValueScope is not ConstantValueScope rightConstant)
            return new AnalysisFailure("Right is not a constant", Location);

        var boolInstance = new BoolInstance(Location, new ConstantValueScope((bool)leftConstant.Value! || (bool)rightConstant.Value!, typeof(bool)), GetNextReferenceId());
        return ImmutableArray.Create((boolInstance as IObjectInstance, state));
    }
}