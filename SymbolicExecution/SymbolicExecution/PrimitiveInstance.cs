namespace SymbolicExecution;

public abstract class PrimitiveInstance<T> : ValueTypeInstance, IPrimitiveInstance<T> where T : struct, IEquatable<T>, IComparable<T>
{
    protected PrimitiveInstance(Location location, IValueScope value, int reference) :
        base(typeof(T), location, value, reference)
    {
    }
}