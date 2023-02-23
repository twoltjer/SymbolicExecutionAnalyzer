namespace SymbolicExecution;

public class FloatInstance : PrimitiveInstance<float>, IFloatInstance
{
    public FloatInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
    {
    }

    public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<float> converted, out AnalysisFailure? analysisFailure)
    {
        converted = default!;
        analysisFailure = new AnalysisFailure("Cannot convert value to float", Location);
        return false;
    }
}