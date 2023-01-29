namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class TypeDeclarationSyntaxAbastraction : BaseTypeDeclarationSyntaxAbstraction, ITypeDeclarationSyntaxAbastraction
{
	protected TypeDeclarationSyntaxAbastraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}