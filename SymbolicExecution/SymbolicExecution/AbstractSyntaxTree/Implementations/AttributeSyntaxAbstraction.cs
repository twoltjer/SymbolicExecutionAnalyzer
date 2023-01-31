namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class AttributeSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IAttributeSyntaxAbstraction
{
	public AttributeSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze attributes", Location);
	}

	public override TaggedUnion<ObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot analyze attributes", Location);
	}
}