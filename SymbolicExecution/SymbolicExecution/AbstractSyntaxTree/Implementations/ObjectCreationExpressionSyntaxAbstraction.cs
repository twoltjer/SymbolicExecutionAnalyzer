namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ObjectCreationExpressionSyntaxAbstraction : BaseObjectCreationExpressionSyntaxAbstraction, IObjectCreationExpressionSyntaxAbstraction
{
	public ObjectCreationExpressionSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}