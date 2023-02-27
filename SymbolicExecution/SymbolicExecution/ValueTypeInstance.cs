namespace SymbolicExecution;

public abstract class ValueTypeInstance : ObjectInstance, IValueTypeInstance
{
    protected ValueTypeInstance(
        TaggedUnion<ITypeSymbol, Type> type,
        Location location,
        IValueScope value,
        int reference
    ) : base(type, location, value, reference)
    {
    }
}