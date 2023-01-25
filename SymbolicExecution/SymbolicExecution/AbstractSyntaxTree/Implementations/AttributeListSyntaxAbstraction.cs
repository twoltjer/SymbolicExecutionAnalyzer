namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class AttributeListSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IAttributeListSyntaxAbstraction
{
	public AttributeListSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}