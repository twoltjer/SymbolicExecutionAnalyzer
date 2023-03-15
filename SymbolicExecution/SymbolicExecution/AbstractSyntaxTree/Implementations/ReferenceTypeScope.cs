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
}