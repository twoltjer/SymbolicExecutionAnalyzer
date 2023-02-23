namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ReferenceTypeScope : IValueScope
{
	private readonly ITypeSymbol _typeSymbol;

	public ReferenceTypeScope(ITypeSymbol typeSymbol)
	{
		_typeSymbol = typeSymbol;
	}

	public bool CouldBe(object? value)
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

	public TaggedUnion<IAnalysisState, AnalysisFailure> ApplyConstraint(IConstraint exactValueConstraint)
	{
		return new AnalysisFailure("Cannot apply constraint to reference type", Location.None);
	}
}