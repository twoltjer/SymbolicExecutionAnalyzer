namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class CSharpSyntaxNodeAbstraction : SyntaxNodeAbstraction, ICSharpSyntaxNodeAbstraction
{
	protected CSharpSyntaxNodeAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}