namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ArgumentListSyntaxAbstraction : BaseArgumentListSyntaxAbstraction, IArgumentListSyntaxAbstraction
{
	public ArgumentListSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<ObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		throw new NotImplementedException();
	}
}