namespace SymbolicExecution;

public class ByteInstance : PrimitiveInstance<byte>, IByteInstance
{
    public ByteInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
    {
    }

    public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<byte> converted, out AnalysisFailure? analysisFailure)
    {
        converted = default!;
        analysisFailure = new AnalysisFailure("Cannot convert value to byte", Location);
        return false;
    }
}