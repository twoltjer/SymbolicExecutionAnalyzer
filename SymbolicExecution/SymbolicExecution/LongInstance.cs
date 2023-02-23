namespace SymbolicExecution;

public class LongInstance : PrimitiveInstance<long>, ILongInstance
{
    public LongInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
    {
    }

    public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<long> converted, out AnalysisFailure? analysisFailure)
    {
        if (value is IIntInstance integerValue)
        {
            if (integerValue.ValueScope is ConstantValueScope constantValue)
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