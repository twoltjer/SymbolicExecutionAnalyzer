namespace SymbolicExecution;

public sealed class ConcreteValueScope<T> : IConcreteValueScope where T : unmanaged
{
	public ConcreteValueScope(T value)
	{
		Value = value;
	}

	public T Value { get; }
	public Result<IValueScope> Union(IValueScope other)
	{
		if (other is ConcreteValueScope<T> otherConcreteValueScope)
		{
			if (Value.Equals(otherConcreteValueScope.Value))
				return new Result<IValueScope>(this);

			return new Result<IValueScope>(EmptyValueScope.Instance);
		}

		return new Result<IValueScope>(new AnalysisErrorInfo($"{nameof(ConcreteValueScope<T>)}.{nameof(Union)}: Could not union scopes", default));
	}

	public Result<IValueScope> Intersection(IValueScope other)
	{
		if (other is ConcreteValueScope<T> otherConcreteValueScope)
		{
			if (otherConcreteValueScope.Value.Equals(Value))
				return new Result<IValueScope>(this);

			return new Result<IValueScope>(EmptyValueScope.Instance);
		}

		return new Result<IValueScope>(new AnalysisErrorInfo($"{nameof(ConcreteValueScope<T>)}.{nameof(Intersection)}: Could not intersect scopes", default));
	}

	public bool Equals(IValueScope other)
	{
		return other is ConcreteValueScope<T> otherConcreteValueScope && Value.Equals(otherConcreteValueScope.Value);
	}
}