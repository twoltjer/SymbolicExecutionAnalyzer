namespace SymbolicExecution;

public class ConstantValueScope : IValueScope
{
	public object? Value { get; }
	private readonly TaggedUnion<ITypeSymbol, Type> _type;

	public ConstantValueScope(object? value, TaggedUnion<ITypeSymbol, Type> type)
	{
		Value = value;
		_type = type;
	}

	public bool CanBe(object? value)
	{
		if (Value == null)
		{
			return value == null;
		}

		return Value.Equals(value);
	}

	public bool IsExactType(Type type)
	{
		return _type.Match(
			t => t.Name == type.Name && t.ContainingNamespace.ToString() == type.Namespace,
			t => t == type
			);
	}

	public bool IsAlways(object? value)
	{
		return CanBe(value);
	}
}