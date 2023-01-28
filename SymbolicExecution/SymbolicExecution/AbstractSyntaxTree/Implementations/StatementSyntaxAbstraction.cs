namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class StatementSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IStatementSyntaxAbstraction
{
	public StatementSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}