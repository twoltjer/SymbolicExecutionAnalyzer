namespace SymbolicExecution;

public interface IValueScope : IEquatable<IValueScope>
{
	Result<IValueScope> Union(IValueScope other);
	Result<IValueScope> Intersection(IValueScope other);
}