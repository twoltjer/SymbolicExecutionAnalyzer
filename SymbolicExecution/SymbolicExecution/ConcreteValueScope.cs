namespace SymbolicExecution;

public sealed class ConcreteValueScope<T> : IConcreteValueScope where T : unmanaged
{
	public ConcreteValueScope(T value)
	{
		Value = value;
	}

	public T Value { get; }
	public IValueScope? Union(IValueScope other)
	{
		if (other is ConcreteValueScope<T> otherConcreteValueScope)
		{
			if (Value.Equals(otherConcreteValueScope.Value))
				return this;

			return EmptyValueScope.Instance;
		}

		throw new UnexpectedValueException();
	}

	public IValueScope? Intersection(IValueScope other)
	{
		if (other is ConcreteValueScope<T> otherConcreteValueScope)
		{
			if (Value.Equals(otherConcreteValueScope.Value))
				return this;

			return EmptyValueScope.Instance;
		}

		throw new UnexpectedValueException();
	}

	public bool Equals(IValueScope other)
	{
		return other is ConcreteValueScope<T> otherConcreteValueScope && Value.Equals(otherConcreteValueScope.Value);
	}
}