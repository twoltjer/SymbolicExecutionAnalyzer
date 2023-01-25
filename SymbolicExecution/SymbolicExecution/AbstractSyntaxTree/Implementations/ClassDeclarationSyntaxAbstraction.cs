namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ClassDeclarationSyntaxAbstraction : TypeDeclarationSyntaxAbastraction, IClassDeclarationSyntaxAbstraction
{
	public ClassDeclarationSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}