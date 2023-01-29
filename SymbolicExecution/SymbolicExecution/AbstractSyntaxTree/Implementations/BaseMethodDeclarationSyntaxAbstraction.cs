namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseMethodDeclarationSyntaxAbstraction : MemberDeclarationSyntaxAbstraction, IBaseMethodDeclarationSyntaxAbstraction
{
	protected BaseMethodDeclarationSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}