namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class StatementSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IStatementSyntaxAbstraction
{
	protected StatementSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}