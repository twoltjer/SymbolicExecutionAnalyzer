namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ParameterListSyntaxAbstraction : BaseParameterListSyntaxAbstraction, IParameterListSyntaxAbstraction
{
	public ParameterListSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}