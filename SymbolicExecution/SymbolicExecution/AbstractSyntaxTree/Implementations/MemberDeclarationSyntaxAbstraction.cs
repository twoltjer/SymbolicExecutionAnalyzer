namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class MemberDeclarationSyntaxAbstraction : CSharpSyntaxNodeAbstraction,
	IMemberDeclarationSyntaxAbstraction
{
	protected MemberDeclarationSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}
}