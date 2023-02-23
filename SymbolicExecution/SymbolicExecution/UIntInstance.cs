namespace SymbolicExecution;

public class UIntInstance : PrimitiveInstance<uint>, IUIntInstance
{
    public UIntInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
    {
    }

    public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<uint> converted, out AnalysisFailure? analysisFailure)
    {
        converted = default!;
        analysisFailure = new AnalysisFailure("Cannot convert value to uint", Location);
        return false;
    }
}