namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class CSharpSyntaxNodeAbstraction : SyntaxNodeAbstraction, ICSharpSyntaxNodeAbstraction
{
	protected CSharpSyntaxNodeAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}