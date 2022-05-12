namespace SymbolicExecution;

public sealed class UninitializedValueScope : IValueScope
{
	public static UninitializedValueScope Instance { get; } = new UninitializedValueScope();

	public IValueScope Union(IValueScope other) =>
		throw new ValidationFailedException("UninitializedValueScope cannot be unioned");

	public IValueScope Intersection(IValueScope other) =>
		throw new ValidationFailedException("UninitializedValueScope cannot be intersected");

	public bool Equals(IValueScope other) => other is UninitializedValueScope;
}