namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class UsingDirectiveSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IUsingDirectiveSyntaxAbstraction
{
	public UsingDirectiveSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}