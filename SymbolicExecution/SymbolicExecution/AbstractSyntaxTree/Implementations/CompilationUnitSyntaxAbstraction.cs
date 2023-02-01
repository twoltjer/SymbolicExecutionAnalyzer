namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class CompilationUnitSyntaxAbstraction : CSharpSyntaxNodeAbstraction, ICompilationUnitSyntaxAbstraction
{
	public CompilationUnitSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze compilation units", Location);
	}

	public override TaggedUnion<IObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot analyze compilation units", Location);
	}
}