namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class MethodDeclarationSyntaxAbstraction : BaseMethodDeclarationSyntaxAbstraction, IMethodDeclarationSyntaxAbstraction
{
	public Location? SourceLocation { get; }

	public MethodDeclarationSyntaxAbstraction(
		ImmutableArray<SyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location? sourceLocation
		) : base(children, symbol)
	{
		SourceLocation = sourceLocation;
	}
}