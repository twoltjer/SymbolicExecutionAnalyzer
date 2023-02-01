namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class MethodDeclarationSyntaxAbstraction : BaseMethodDeclarationSyntaxAbstraction, IMethodDeclarationSyntaxAbstraction
{
	public MethodDeclarationSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		throw new NotImplementedException();
	}

	public override TaggedUnion<IObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		throw new NotImplementedException();
	}
}