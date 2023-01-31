namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class QualifiedNameSyntaxAbstraction : NameSyntaxAbstraction, IQualifiedNameSyntaxAbstraction
{
	public QualifiedNameSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze qualified names", Location);
	}

	public override TaggedUnion<ObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot analyze qualified names", Location);
	}
}