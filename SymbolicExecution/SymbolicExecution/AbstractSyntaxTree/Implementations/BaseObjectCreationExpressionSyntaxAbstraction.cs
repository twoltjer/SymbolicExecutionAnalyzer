namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class BaseObjectCreationExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction, IBaseObjectCreationExpressionSyntaxAbstraction
{
	public BaseObjectCreationExpressionSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}