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

	protected bool Equals(ConstantValueScope other)
	{
		var typeName = Type.Match(t1 => t1.Name, t2 => t2.Name);
		var typeNamespace = Type.Match(t1 => t1.ContainingNamespace.ToString(), t2 => t2.Namespace);
		var otherTypeName = other.Type.Match(t1 => t1.Name, t2 => t2.Name);
		var otherTypeNamespace = other.Type.Match(t1 => t1.ContainingNamespace.ToString(), t2 => t2.Namespace);
		return Value == other.Value && typeName == otherTypeName && typeNamespace == otherTypeNamespace;
	}

	public bool Equals(IValueScope other)
	{
		return other is ConstantValueScope constantValueScope && Equals(constantValueScope);
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((ConstantValueScope)obj);
	}

	public override int GetHashCode()
	{
		var typeName = Type.Match(t1 => t1.Name, t2 => t2.Name);
		var typeNamespace = Type.Match(t1 => t1.ContainingNamespace.ToString(), t2 => t2.Namespace);
		return (Value, typeName, typeNamespace).GetHashCode();
	}
}