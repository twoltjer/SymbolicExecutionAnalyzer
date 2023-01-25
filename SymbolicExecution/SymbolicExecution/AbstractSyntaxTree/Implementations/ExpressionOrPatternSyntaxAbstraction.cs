namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ExpressionOrPatternSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IExpressionOrPatternSyntaxAbstraction
{
	public ExpressionOrPatternSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}