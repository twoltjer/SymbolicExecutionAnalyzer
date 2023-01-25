namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class BaseParameterListSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IBaseParameterListSyntaxAbstraction
{
	public BaseParameterListSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}