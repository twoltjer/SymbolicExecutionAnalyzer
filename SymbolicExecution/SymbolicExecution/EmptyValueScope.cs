namespace SymbolicExecution;

internal sealed class EmptyValueScope : IValueScope
{
	internal static readonly EmptyValueScope Instance = new EmptyValueScope();

	private EmptyValueScope()
	{
	}

	public IValueScope Union(IValueScope other) => this;

	public IValueScope Intersection(IValueScope other) => other;

	public bool Equals(IValueScope other) => other is EmptyValueScope;
}