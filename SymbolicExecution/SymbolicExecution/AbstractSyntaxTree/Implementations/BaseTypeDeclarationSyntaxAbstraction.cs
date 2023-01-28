namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseTypeDeclarationSyntaxAbstraction : MemberDeclarationSyntaxAbstraction, IBaseTypeDeclarationSyntaxAbstraction
{
	public BaseTypeDeclarationSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}