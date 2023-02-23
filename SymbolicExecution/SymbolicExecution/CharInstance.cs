namespace SymbolicExecution;

public class CharInstance : PrimitiveInstance<char>, ICharInstance
{
    public CharInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
    {
    }

    public override bool TryConvert(IObjectInstance value, out IPrimitiveInstance<char> converted, out AnalysisFailure? analysisFailure)
    {
        converted = default!;
        analysisFailure = new AnalysisFailure("Cannot convert value to char", Location);
        return false;
    }
}