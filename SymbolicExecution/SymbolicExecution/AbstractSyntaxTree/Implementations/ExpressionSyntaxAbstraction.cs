namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class ExpressionSyntaxAbstraction : ExpressionOrPatternSyntaxAbstraction, IExpressionSyntaxAbstraction
{
	public ExpressionSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}