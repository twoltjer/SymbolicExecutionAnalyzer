namespace SymbolicExecution;

internal sealed class EmptyValueScope : IValueScope
{
	internal static readonly EmptyValueScope Instance = new EmptyValueScope();

	private EmptyValueScope()
	{
	}

	public Result<IValueScope> Union(IValueScope other) => new Result<IValueScope>(this);

	public Result<IValueScope> Intersection(IValueScope other) => new Result<IValueScope>(other);

	public bool Equals(IValueScope other) => other is EmptyValueScope;
}