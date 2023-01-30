namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseTypeDeclarationSyntaxAbstraction : MemberDeclarationSyntaxAbstraction, IBaseTypeDeclarationSyntaxAbstraction
{
	protected BaseTypeDeclarationSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}