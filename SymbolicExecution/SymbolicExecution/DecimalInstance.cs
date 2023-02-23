namespace SymbolicExecution;

public class DecimalInstance : PrimitiveInstance<decimal>, IDecimalInstance
{
    public DecimalInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
    {
    }

    public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<decimal> converted, out AnalysisFailure? analysisFailure)
    {
        converted = default!;
        analysisFailure = new AnalysisFailure("Cannot convert value to decimal", Location);
        return false;
    }
}