namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class TypeDeclarationSyntaxAbastraction : BaseTypeDeclarationSyntaxAbstraction, ITypeDeclarationSyntaxAbastraction
{
	protected TypeDeclarationSyntaxAbastraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}