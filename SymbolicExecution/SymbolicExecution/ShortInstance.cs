namespace SymbolicExecution;

public class ShortInstance : PrimitiveInstance<short>, IShortInstance
{
    public ShortInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
    {
    }

    public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<short> converted, out AnalysisFailure? analysisFailure)
    {
        converted = default!;
        analysisFailure = new AnalysisFailure("Cannot convert value to short", Location);
        return false;
    }
}