namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ArgumentListSyntaxAbstraction : BaseArgumentListSyntaxAbstraction, IArgumentListSyntaxAbstraction
{
	public ArgumentListSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze argument lists", Location);
	}
}