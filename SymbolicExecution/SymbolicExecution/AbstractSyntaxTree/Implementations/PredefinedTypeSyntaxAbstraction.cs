namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class PredefinedTypeSyntaxAbstraction : TypeSyntaxAbstraction, IPredefinedTypeSyntaxAbstraction
{
	public PredefinedTypeSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze predefined types", Location);
	}

	public override TaggedUnion<ObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot analyze predefined types", Location);
	}
}