namespace SymbolicExecution;

public class ULongInstance : PrimitiveInstance<ulong>, IULongInstance
{
    public ULongInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
    {
    }

    public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<ulong> converted, out AnalysisFailure? analysisFailure)
    {
        converted = default!;
        analysisFailure = new AnalysisFailure("Cannot convert value to ulong", Location);
        return false;
    }
}