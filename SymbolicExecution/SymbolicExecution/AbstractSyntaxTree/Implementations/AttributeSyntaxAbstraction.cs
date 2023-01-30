namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class AttributeSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IAttributeSyntaxAbstraction
{
	public AttributeSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
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