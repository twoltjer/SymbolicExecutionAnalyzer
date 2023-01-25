namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ArgumentListSyntaxAbstraction : BaseArgumentListSyntaxAbstraction, IArgumentListSyntaxAbstraction
{
	public ArgumentListSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}