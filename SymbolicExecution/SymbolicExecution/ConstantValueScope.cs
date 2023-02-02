namespace SymbolicExecution;

public class ConstantValueScope : IValueScope
{
	private readonly object? _value;
	private readonly ITypeSymbol _type;

	public ConstantValueScope(object? value, ITypeSymbol type)
	{
		_value = value;
		_type = type;
	}

	public bool CanBe(object? value)
	{
		if (_value == null)
		{
			return value == null;
		}

		return _value.Equals(value);
	}

	public bool IsExactType(Type type)
	{
		return _type.Name == type.Name && _type.ContainingNamespace.ToString() == type.Namespace;
	}

	public bool IsAlways(object? value)
	{
		return CanBe(value);
	}
}