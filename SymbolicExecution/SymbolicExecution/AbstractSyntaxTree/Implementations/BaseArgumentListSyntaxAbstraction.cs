namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseArgumentListSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IBaseArgumentListSyntaxAbstraction
{
	protected BaseArgumentListSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}