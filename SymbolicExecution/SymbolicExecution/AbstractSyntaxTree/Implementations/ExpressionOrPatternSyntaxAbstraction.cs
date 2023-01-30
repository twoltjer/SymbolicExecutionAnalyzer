namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class ExpressionOrPatternSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IExpressionOrPatternSyntaxAbstraction
{
	protected ExpressionOrPatternSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}