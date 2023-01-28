namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class MemberDeclarationSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IMemberDeclarationSyntaxAbstraction
{
	public MemberDeclarationSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}