namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class CSharpSyntaxNodeAbstraction : SyntaxNodeAbstraction, ICSharpSyntaxNodeAbstraction
{
	public CSharpSyntaxNodeAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}