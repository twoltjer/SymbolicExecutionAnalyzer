namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class UsingDirectiveSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IUsingDirectiveSyntaxAbstraction
{
	public UsingDirectiveSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze using directives", Location);
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot analyze using directives", Location);
	}
}