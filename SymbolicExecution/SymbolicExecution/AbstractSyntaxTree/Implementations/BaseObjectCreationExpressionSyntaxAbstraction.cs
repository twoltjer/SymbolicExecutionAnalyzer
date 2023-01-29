namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseObjectCreationExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction, IBaseObjectCreationExpressionSyntaxAbstraction
{
	protected BaseObjectCreationExpressionSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}