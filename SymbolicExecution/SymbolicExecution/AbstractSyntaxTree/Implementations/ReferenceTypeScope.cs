namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ReferenceTypeScope : IValueScope
{
	private readonly TaggedUnion<ITypeSymbol, Type> _typeSymbol;

	public ReferenceTypeScope(TaggedUnion<ITypeSymbol, Type> typeSymbol)
	{
		_typeSymbol = typeSymbol;
	}

	public bool CanBe(object? value)
	{
		return false;
	}

	public bool IsExactType(Type type)
	{
		return _typeSymbol.Match(t1 => t1.Name, t2 => t2.Name) == type.Name && _typeSymbol.Match(t1 => t1.ContainingNamespace.ToString(), t2 => t2.Namespace) == type.Namespace;
	}

	public bool IsAlways(object? value)
	{
		return false;
	}

	protected bool Equals(ReferenceTypeScope other)
	{
		var typeName = _typeSymbol.Match(t1 => t1.Name, t2 => t2.Name);
		var typeNamespace = _typeSymbol.Match(t1 => t1.ContainingNamespace.ToString(), t2 => t2.Namespace);
		var otherTypeName = other._typeSymbol.Match(t1 => t1.Name, t2 => t2.Name);
		var otherTypeNamespace = other._typeSymbol.Match(t1 => t1.ContainingNamespace.ToString(), t2 => t2.Namespace);
		return typeName == otherTypeName && typeNamespace == otherTypeNamespace;
	}

	public bool Equals(IValueScope other)
	{
		return other is ReferenceTypeScope referenceTypeScope && Equals(referenceTypeScope);
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((ReferenceTypeScope)obj);
	}

	public override int GetHashCode()
	{
		var typeName = _typeSymbol.Match(t1 => t1.Name, t2 => t2.Name);
		var typeNamespace = _typeSymbol.Match(t1 => t1.ContainingNamespace.ToString(), t2 => t2.Namespace);
		return (typeName, typeNamespace).GetHashCode();
	}
}