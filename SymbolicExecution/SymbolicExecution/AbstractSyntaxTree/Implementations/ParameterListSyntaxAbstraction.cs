namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ParameterListSyntaxAbstraction : BaseParameterListSyntaxAbstraction, IParameterListSyntaxAbstraction
{
	public ParameterListSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze parameter lists", Location);
	}

	public override TaggedUnion<IObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot analyze parameter lists", Location);
	}
}