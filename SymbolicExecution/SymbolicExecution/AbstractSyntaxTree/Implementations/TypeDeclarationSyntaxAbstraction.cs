namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class TypeDeclarationSyntaxAbstraction : BaseTypeDeclarationSyntaxAbstraction,
	ITypeDeclarationSyntaxAbstraction
{
	protected TypeDeclarationSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}
}