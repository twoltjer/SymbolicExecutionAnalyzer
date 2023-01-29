namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseTypeDeclarationSyntaxAbstraction : MemberDeclarationSyntaxAbstraction, IBaseTypeDeclarationSyntaxAbstraction
{
	protected BaseTypeDeclarationSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}