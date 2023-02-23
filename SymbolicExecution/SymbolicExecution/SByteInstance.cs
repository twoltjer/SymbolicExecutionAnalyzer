namespace SymbolicExecution;

public class SByteInstance : PrimitiveInstance<sbyte>, ISByteInstance
{
    public SByteInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
    {
    }

    public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<sbyte> converted, out AnalysisFailure? analysisFailure)
    {
        converted = default!;
        analysisFailure = new AnalysisFailure("Cannot convert value to sbyte", Location);
        return false;
    }
}