namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class CompilationUnitSyntaxAbstraction : CSharpSyntaxNodeAbstraction, ICompilationUnitSyntaxAbstraction
{
	public CompilationUnitSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}