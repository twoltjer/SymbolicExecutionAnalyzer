namespace SymbolicExecution;

public class UShortInstance : PrimitiveInstance<ushort>, IUShortInstance
{
    public UShortInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
    {
    }

    public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<ushort> converted, out AnalysisFailure? analysisFailure)
    {
        converted = default!;
        analysisFailure = new AnalysisFailure("Cannot convert value to ushort", Location);
        return false;
    }
}