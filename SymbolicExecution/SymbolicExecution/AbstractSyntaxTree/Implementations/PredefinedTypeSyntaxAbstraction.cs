namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class PredefinedTypeSyntaxAbstraction : TypeSyntaxAbstraction, IPredefinedTypeSyntaxAbstraction
{
	public PredefinedTypeSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? type
		) : base(children, symbol, location, type)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze predefined types", Location);
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot analyze predefined types", Location);
	}
}