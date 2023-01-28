namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class QualifiedNameSyntaxAbstraction : NameSyntaxAbstraction, IQualifiedNameSyntaxAbstraction
{
	public QualifiedNameSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
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