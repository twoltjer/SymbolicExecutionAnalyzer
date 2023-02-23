namespace SymbolicExecution;

public class ValueTypeInstance : ObjectInstance, IValueTypeInstance
{
    public ValueTypeInstance(
        TaggedUnion<ITypeSymbol, Type> type,
        Location location,
        IValueScope value,
        int reference
    ) : base(type, location, value, reference)
    {
    }
}