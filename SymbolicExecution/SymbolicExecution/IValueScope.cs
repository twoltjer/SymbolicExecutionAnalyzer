namespace SymbolicExecution;

public interface IValueScope : IEquatable<IValueScope>
{
	IValueScope? Union(IValueScope other);
	IValueScope? Intersection(IValueScope other);
}