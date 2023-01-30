namespace SymbolicExecution.AbstractSyntaxTree.Interfaces;

public class IdentifierNameSyntaxAbstraction : SimpleNameSyntaxAbstraction, IIdentifierNameSyntaxAbstraction
{
	public IdentifierNameSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new[] { previous };
	}

	public override TaggedUnion<ObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot get the result of an identifier name", Location.None);
	}
}