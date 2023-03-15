namespace SymbolicExecution;

public class ConstantValueScope : IValueScope
{
	public object? Value { get; }

	public TaggedUnion<ITypeSymbol, Type> Type { get; }

	public ConstantValueScope(object? value, TaggedUnion<ITypeSymbol, Type> type)
	{
		Value = value;
		Type = type;
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
		return Type.Match(t1 => t1.Name, t2 => t2.Name) == type.Name && Type.Match(t1 => t1.ContainingNamespace.ToString(), t2 => t2.Namespace) == type.Namespace;
	}

	public bool IsAlways(object? value)
	{
		return CanBe(value);
	}
	
	public override string ToString()
	{
		return Value?.ToString() ?? "null";
	}
}