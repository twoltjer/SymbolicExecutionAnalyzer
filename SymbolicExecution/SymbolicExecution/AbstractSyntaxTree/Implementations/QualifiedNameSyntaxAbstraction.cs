namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class QualifiedNameSyntaxAbstraction : NameSyntaxAbstraction, IQualifiedNameSyntaxAbstraction
{
	public QualifiedNameSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}