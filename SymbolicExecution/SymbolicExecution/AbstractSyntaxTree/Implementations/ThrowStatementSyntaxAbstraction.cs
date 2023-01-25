namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ThrowStatementSyntaxAbstraction : StatementSyntaxAbstraction, IThrowStatementSyntaxAbstraction
{
	public ThrowStatementSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}