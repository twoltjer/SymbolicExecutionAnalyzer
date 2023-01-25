namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class BaseTypeDeclarationSyntaxAbstraction : MemberDeclarationSyntaxAbstraction, IBaseTypeDeclarationSyntaxAbstraction
{
	public BaseTypeDeclarationSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}