namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class AttributeSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IAttributeSyntaxAbstraction
{
	public AttributeSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}