namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseTypeDeclarationSyntaxAbstraction : MemberDeclarationSyntaxAbstraction,
	IBaseTypeDeclarationSyntaxAbstraction
{
	protected BaseTypeDeclarationSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}
}