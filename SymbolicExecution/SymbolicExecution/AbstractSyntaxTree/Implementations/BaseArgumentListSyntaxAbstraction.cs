namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseArgumentListSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IBaseArgumentListSyntaxAbstraction
{
	protected BaseArgumentListSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}