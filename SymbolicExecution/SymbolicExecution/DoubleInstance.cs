namespace SymbolicExecution;

public class DoubleInstance : PrimitiveInstance<double>, IDoubleInstance
{
    public DoubleInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
    {
    }

    public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<double> converted, out AnalysisFailure? analysisFailure)
    {
        converted = default!;
        analysisFailure = new AnalysisFailure("Cannot convert value to double", Location);
        return false;
    }
}