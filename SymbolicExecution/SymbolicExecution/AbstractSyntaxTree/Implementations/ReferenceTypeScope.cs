namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ReferenceTypeScope : IValueScope
{
	private readonly ITypeSymbol _typeSymbol;

	public ReferenceTypeScope(ITypeSymbol typeSymbol)
	{
		_typeSymbol = typeSymbol;
	}

	public bool CanBe(object? value)
	{
		return false;
	}

	public bool IsExactType(Type type)
	{
		return _typeSymbol.Name == type.Name && _typeSymbol.ContainingNamespace.ToString() == type.Namespace;
	}

	public bool IsAlways(object? value)
	{
		return false;
	}
}