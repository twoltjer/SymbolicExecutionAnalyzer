namespace SymbolicExecution.AbstractSyntaxTree.Interfaces;

public class IdentifierNameSyntaxAbstraction : SimpleNameSyntaxAbstraction, IIdentifierNameSyntaxAbstraction
{
	public IdentifierNameSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}