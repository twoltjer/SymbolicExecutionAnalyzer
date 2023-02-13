namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class EqualsValueClauseSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IEqualsValueClauseSyntaxAbstraction
{
	public EqualsValueClauseSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze an equals value clause", Location);
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot get the result of an equals value clause", Location);
	}
}