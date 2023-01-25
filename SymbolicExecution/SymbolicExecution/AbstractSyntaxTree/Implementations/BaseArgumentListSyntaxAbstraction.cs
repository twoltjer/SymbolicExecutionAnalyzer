namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class BaseArgumentListSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IBaseArgumentListSyntaxAbstraction
{
	public BaseArgumentListSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}