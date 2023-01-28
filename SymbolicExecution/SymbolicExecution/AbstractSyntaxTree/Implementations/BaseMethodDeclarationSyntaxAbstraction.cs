namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseMethodDeclarationSyntaxAbstraction : MemberDeclarationSyntaxAbstraction, IBaseMethodDeclarationSyntaxAbstraction
{
	public BaseMethodDeclarationSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}