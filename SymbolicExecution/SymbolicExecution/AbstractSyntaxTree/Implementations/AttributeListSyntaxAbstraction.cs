namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class AttributeListSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IAttributeListSyntaxAbstraction
{
	public AttributeListSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze attribute lists", Location);
	}

	public override TaggedUnion<IObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot analyze attribute lists", Location);
	}
}