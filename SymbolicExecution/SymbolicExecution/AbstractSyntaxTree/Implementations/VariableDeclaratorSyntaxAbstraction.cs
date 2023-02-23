namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class VariableDeclaratorSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IVariableDeclaratorSyntaxAbstraction
{
	public VariableDeclaratorSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze a variable declarator", Location);
	}
}