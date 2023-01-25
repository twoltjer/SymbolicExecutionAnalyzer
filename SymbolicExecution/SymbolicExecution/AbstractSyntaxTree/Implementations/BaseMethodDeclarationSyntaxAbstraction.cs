namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class BaseMethodDeclarationSyntaxAbstraction : MemberDeclarationSyntaxAbstraction, IBaseMethodDeclarationSyntaxAbstraction
{
	public BaseMethodDeclarationSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}